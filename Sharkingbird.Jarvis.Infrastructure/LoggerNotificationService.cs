using Microsoft.Extensions.Logging;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure
{
  public sealed class LoggerNotificationService : INotificationService
  {
    private readonly ILogger<LoggerNotificationService> _logger;
    public LoggerNotificationService(ILogger<LoggerNotificationService> logger)
    {
      _logger = logger;
    }
    public Task PushNotifications(IEnumerable<Notification> notifications, CancellationToken cancellationToken)
    {
      foreach(var n in notifications)
      {
        _logger.LogWarning(n.Message);
      }
      return Task.CompletedTask;
    }
  }
}
