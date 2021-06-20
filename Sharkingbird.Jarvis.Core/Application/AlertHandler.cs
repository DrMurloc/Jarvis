using System.Collections.Generic;
using System.Linq;
using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Core.Models.Events;
using System.Threading;
using System.Threading.Tasks;
using Sharkingbird.Jarvis.Core.Models.Discretionary;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class AlertHandler : INotificationHandler<TransactionsAppliedEvent>,
    INotificationHandler<DiscretionaryBudgetBalanceModifiedEvent>,
    INotificationHandler<VacationBudgetBalanceModifiedEvent>,
    INotificationHandler<VacationTransactionsAppliedOutsideOfVacationEvent>
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

    public async Task Handle(VacationTransactionsAppliedOutsideOfVacationEvent notification,
      CancellationToken cancellationToken)
    {
      var transactionMessages = BuildTransactionMessage(notification.Transactions);
      await _notificationService.PushNotification(
        new Notification($"Vacation Transactions Detected Outside of a Vacation: {transactionMessages}"),cancellationToken);
    }
    private static string BuildTransactionMessage(IEnumerable<Transaction> transactions) => string.Join('\n', transactions.Select(t => $"{t.Description} for ${t.Amount}"));
    public async Task Handle(TransactionsAppliedEvent notification, CancellationToken cancellationToken)
    {
      var transactionMessages = BuildTransactionMessage(notification.Transactions);
      await _notificationService.PushNotification(new Notification($"Discretionary Transaction Applied: {transactionMessages}"),cancellationToken);
    }
  }
}
