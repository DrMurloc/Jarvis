using Microsoft.Extensions.Options;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Sharkingbird.Jarvis.Infrastructure
{
  public sealed class TwilioNotificationService : INotificationService
  {
    private TwilioConfiguration _configuration;
    public TwilioNotificationService(IOptions<TwilioConfiguration> options)
    {
      _configuration = options.Value;
      TwilioClient.Init(_configuration.AccountId, _configuration.AuthToken);
    }
    public async Task PushNotifications(IEnumerable<Notification> notifications, CancellationToken cancellationToken)
    {
      foreach(var n in notifications)
      foreach(var p in _configuration.ToPhoneNumbers)
      {
          await MessageResource.CreateAsync(
              body: n.Message,
              from: new Twilio.Types.PhoneNumber(_configuration.FromPhoneNumber),
              to: new Twilio.Types.PhoneNumber(p)
          );
      }
    }
  }
}
