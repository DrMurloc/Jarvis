using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Core.Models.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class AlertHandler : INotificationHandler<TransactionAppliedEvent>,
    INotificationHandler<DiscretionaryBudgetBalanceModifiedEvent>,
    INotificationHandler<VacationBudgetBalanceModifiedEvent>
  {
    private INotificationService _notificationService;
    public AlertHandler(INotificationService notificationService)
    {
      _notificationService = notificationService;
    }

    public async Task Handle(DiscretionaryBudgetBalanceModifiedEvent notification, CancellationToken cancellationToken)
    {
      await _notificationService.PushNotification(new Notification($"New Discretionary Balance: " + notification.NewBalance), cancellationToken);
    }

    public async Task Handle(VacationBudgetBalanceModifiedEvent notification, CancellationToken cancellationToken)
    {
      await _notificationService.PushNotification(new Notification($"New Vacation Balance: " + notification.NewBalance), cancellationToken);
    }

    public async Task Handle(TransactionAppliedEvent notification, CancellationToken cancellationToken)
    {
      await _notificationService.PushNotification(new Notification($"Discretionary Transaction Applied: {notification.Transaction.Description} for ${notification.Transaction.Amount}"),cancellationToken);
    }
  }
}
