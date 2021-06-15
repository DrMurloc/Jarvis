using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Events;
using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class TransactionNotificationHandler : INotificationHandler<TransactionsAppliedEvent>
  {
    private INotificationService _notificationService;
    public TransactionNotificationHandler(INotificationService notificationService)
    {
      _notificationService = notificationService;
    }
    public async Task Handle(TransactionsAppliedEvent notification, CancellationToken cancellationToken)
    {
      var budget = notification.Budget;
      var diff = budget.NewTransactions.Sum(b => b.Amount);
      var names = string.Join(", ", budget.NewTransactions.Select(t => t.Description).OrderBy(d => d));
      await _notificationService.PushNotification(new Notification($"{names} applied to {budget.Name} for ${diff}. Balance: {budget.Balance}"), cancellationToken);
    }
  }
}
