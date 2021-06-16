using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure
{
  public sealed class CalenderEventRepository : ICalendarEventRepository
  {
    private readonly GoogleConfiguration _configuration;
    public CalenderEventRepository(IOptions<GoogleConfiguration> options)
    {
      _configuration = options.Value;
    }
    public async Task<IEnumerable<Vacation>> GetUpcomingCalanderEvents(CancellationToken cancellationToken)
    {
      var credentialJson = new
      {
        type = "service_account",
        project_id = _configuration.ProjectId,
        private_key_id = _configuration.PrivateKeyId,
        private_key = _configuration.PrivateKey.Replace("\\n","\n"),
        client_email = _configuration.ClientEmail,
        client_id = _configuration.ClientId,
        auth_uri = "https://accounts.google.com/o/oauth2/auth",
        token_uri = "https://oauth2.googleapis.com/token",
        auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs",
        client_x509_cert_url = "https://www.googleapis.com/robot/v1/metadata/x509/" + Uri.EscapeUriString(_configuration.ClientEmail)
      };
      var credentials = GoogleCredential.FromJson(JsonConvert.SerializeObject(credentialJson)).CreateScoped(new[] { CalendarService.Scope.Calendar });

      var service = new CalendarService(new BaseClientService.Initializer()
      {
        ApplicationName = "Sharkingbird Jarvis",
        HttpClientInitializer = credentials
      });
      var eventRequest = service.Events.List(_configuration.CalendarId);
      eventRequest.MaxResults = 10;
      var result = await eventRequest.ExecuteAsync();
      return result.Items.Select(i => new Vacation(HashToGuid(i.Id), i.Summary, DateTimeOffset.Parse(i.Start.Date), DateTimeOffset.Parse(i.End.Date))).ToArray();
    }
    private Guid HashToGuid(string input)
    {
      using (MD5 md5 = MD5.Create())
      {
        byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(input));
        return new Guid(hash);
      }
    }
  }
}
