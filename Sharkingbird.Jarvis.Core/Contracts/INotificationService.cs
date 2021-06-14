using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Contracts
{
  public interface INotificationService
  {
    Task PushNotifications(IEnumerable<Notification> notifications, CancellationToken cancellationToken);
    Task PushNotification(Notification notification, CancellationToken cancellationToken) => PushNotifications(new[] { notification }, cancellationToken);
  }
}
