using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sharkingbird.Jarvis.Core.Models.Vacation;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using MediatR;
using Sharkingbird.Jarvis.Core.Models.Events;

namespace Sharkingbird.Jarvis.Infrastructure
{
  public sealed class VacationRepository : IVacationRepository,
    INotificationHandler<VacationExpensesModifiedEvent>
  {
    private readonly GoogleConfiguration _configuration;
    private readonly ILogger<VacationRepository> _logger;
    private readonly IMediator _mediator;
    private CalendarService _calendarService;
    public VacationRepository(IOptions<GoogleConfiguration> options,
      ILogger<VacationRepository> logger,
      IMediator mediator)
    {
      _configuration = options.Value;
      _logger = logger;
      _mediator = mediator;
    }

    private CalendarService BuildCalendarService()
    {
      if (_calendarService != null)
      {
        return _calendarService;
      }
      var credentialJson = new
      {
        type = "service_account",
        project_id = _configuration.ProjectId,
        private_key_id = _configuration.PrivateKeyId,
        private_key = _configuration.PrivateKey.Replace("\\n", "\n"),
        client_email = _configuration.ClientEmail,
        client_id = _configuration.ClientId,
        auth_uri = "https://accounts.google.com/o/oauth2/auth",
        token_uri = "https://oauth2.googleapis.com/token",
        auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs",
        client_x509_cert_url = "https://www.googleapis.com/robot/v1/metadata/x509/" + Uri.EscapeUriString(_configuration.ClientEmail)
      };
      var credentials = GoogleCredential.FromJson(JsonConvert.SerializeObject(credentialJson)).CreateScoped(new[] { CalendarService.Scope.Calendar });

      return _calendarService = new CalendarService(new BaseClientService.Initializer()
      {
        ApplicationName = "Sharkingbird Jarvis",
        HttpClientInitializer = credentials
      });
    }
    private async Task<IList<Event>> GetCalanderEvents(CancellationToken cancellationToken)
    {
      var service = BuildCalendarService();
      var eventRequest = service.Events.List(_configuration.CalendarId);
      return (await eventRequest.ExecuteAsync(cancellationToken)).Items;
    }
    public async Task<IEnumerable<Vacation>> GetUpcomingCalanderEvents(CancellationToken cancellationToken)
    {
      var events = await GetCalanderEvents(cancellationToken);
      return events.Select(i => new Vacation(_mediator, GetEventId(i), i.Summary, DateTimeOffset.Parse(i.Start.Date), DateTimeOffset.Parse(i.End.Date),GetExpensesFromDescription(i.Description))).ToArray();
    }
    private static readonly Regex MoneyRegex = new Regex(@"[0-9\.]{1,}", RegexOptions.Compiled);
    private IEnumerable<VacationExpense> GetExpensesFromDescription(string description)
    {
      var lines = description.Split('\n');
      return lines.Select(line =>
      {
        try
        {
          var split = line.Split("=>").Select(l => l.Trim()).ToArray();
          var name = split[0];
          var amount = decimal.Parse(MoneyRegex.Match(split[1]).Value);
          var isPaid = split[1].Contains("paid", StringComparison.OrdinalIgnoreCase);
          return new VacationExpense(name, isPaid, amount);
        }
        catch (Exception e)
        {
          _logger.LogError(e, "Error parsing vacation expense");
          return null;
        }
      }).Where(v => v != null);
    }
    private Guid GetEventId(Event e)
    {
      var id = e.Id;
      using var md5 = MD5.Create();
      var hash = md5.ComputeHash(Encoding.Default.GetBytes(id));
      return new Guid(hash);
    }

    public async Task Handle(VacationExpensesModifiedEvent notification, CancellationToken cancellationToken)
    {
      var events = await GetCalanderEvents(cancellationToken);
      var matchedEvent = events.Single(e => GetEventId(e) == notification.Vacation.Id);
      var service = BuildCalendarService();
      var description = string.Join("\n",
        notification.Vacation.Expenses.Select(e => $"{e.Name} => {e.Amount}{(e.IsPaid ? " (Paid)" : "")}"));

      matchedEvent.Description = description;
      await service.Events.Update(matchedEvent, _configuration.CalendarId, matchedEvent.Id).ExecuteAsync(cancellationToken);
    }
  }
}
