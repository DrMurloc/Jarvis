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
  public sealed class GoogleCalendarVacationRepository : IVacationRepository,
    INotificationHandler<VacationExpensesModifiedEvent>
  {
    private readonly GoogleConfiguration _configuration;
    private readonly ILogger<GoogleCalendarVacationRepository> _logger;
    private readonly IMediator _mediator;
    private CalendarService _calendarService;
    public GoogleCalendarVacationRepository(IOptions<GoogleConfiguration> options,
      ILogger<GoogleCalendarVacationRepository> logger,
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
      var credentials = GoogleCredential.FromJson(JsonConvert.SerializeObject(credentialJson)).CreateScoped(new[] { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents });

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
      return events.Select(GetVacationFromEvent).ToArray();
    }
    private const string DailyEstimateName = "Daily Estimate: ";
    private static readonly Regex MoneyRegex = new Regex(@"[0-9\.]{1,}", RegexOptions.Compiled);
    private static readonly Regex PerDayRegex = new Regex(@"([0-9\.]{1,})\s*Per\s*Day", RegexOptions.Compiled);
    private Vacation GetVacationFromEvent(Event e)
    {
      var startDate = DateTimeOffset.Parse(e.Start.Date ?? e.Start.DateTimeRaw);
      var endDate = DateTimeOffset.Parse(e.End.Date ?? e.End.DateTimeRaw);
      return new Vacation(_mediator, GetEventId(e), e.Summary, startDate,
        endDate, GetExpensesFromDescription(startDate,endDate,e.Description ?? ""));
    }
    
    private IEnumerable<VacationExpense> GetExpensesFromDescription(DateTimeOffset startDate, DateTimeOffset endDate, string description)
    {
      var lines = description.Split('\n');
      return lines.Select(line =>
      {
        try
        {
          string name;
          decimal amount;
          bool isPaid;
          var perDayMatch = PerDayRegex.Match(line);
          if (perDayMatch.Success)
          {
            var dailyAmount = decimal.Parse(perDayMatch.Groups[1].Value);
            name = DailyEstimateName + dailyAmount;
            isPaid = false;
            var totalDays = (int) Math.Ceiling((endDate - startDate).TotalDays);
            amount = dailyAmount * totalDays;
          } else
          {
            var split = line.Split("=>").Select(l => l.Trim()).ToArray();
            name = split[0];
            amount = decimal.Parse(MoneyRegex.Match(split[1]).Value);
            isPaid = split[1].Contains("paid", StringComparison.OrdinalIgnoreCase);
          }

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

    private string GetDescriptionFromExpenses(IEnumerable<VacationExpense> expenses)
    {
      var result = "";
      foreach(var expense in expenses)
      {
        if(expense.Name.StartsWith(DailyEstimateName))
        {
          var moneyMatch = MoneyRegex.Match(expense.Name).Groups[0].Value;
          result += moneyMatch + " Per Day";
        }else
        {
          result += $"{expense.Name} => {expense.Amount}{(expense.IsPaid ? " (Paid)" : "")}";
        }
        result += "\n";
      }
      return result[0..^1];
    }
    public async Task Handle(VacationExpensesModifiedEvent notification, CancellationToken cancellationToken)
    {
      var events = await GetCalanderEvents(cancellationToken);
      var matchedEvent = events.Single(e => GetEventId(e) == notification.Vacation.Id);
      var service = BuildCalendarService();
      var description = GetDescriptionFromExpenses(notification.Vacation.Expenses);
      matchedEvent.Description = description; 
      var update = service.Events.Update(matchedEvent, _configuration.CalendarId, matchedEvent.Id);
      await update.ExecuteAsync(cancellationToken);
    }
  }
}
