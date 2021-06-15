using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using Sharkingbird.Jarvis.Core.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class ApplyRecurringTransactionsHandler : IRequestHandler<ApplyRecurringTransactionsCommand>
  {
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly INotificationService _notificationService;
    public ApplyRecurringTransactionsHandler(ITransactionRepository transactionRepository,
      IBudgetRepository budgetRepository,
      INotificationService notificationService)
    {
      _transactionRepository = transactionRepository;
      _budgetRepository = budgetRepository;
      _notificationService = notificationService;
    }
    public async Task<Unit> Handle(ApplyRecurringTransactionsCommand request, CancellationToken cancellationToken)
    {
      var recurringPayments = (await _transactionRepository.GetRecurringTransactions(cancellationToken)).ToArray();
      var budgets = await _budgetRepository.GetBudgets(cancellationToken);
      foreach(var budget in budgets)
      {
        budget.ApplyRecurringTransactions(recurringPayments);
        if (!budget.NewTransactions.Any())
        {
          continue;
        }
        await _budgetRepository.SaveBudget(budget, cancellationToken);
        var diff = budget.NewTransactions.Sum(b => b.Amount);
        var names = string.Join(", ", budget.NewTransactions.Select(t => t.Description).OrderBy(d => d));
        await _notificationService.PushNotification(new Notification($"{names} applied to {budget.Name} for ${diff}. Balance: {budget.Balance}"),cancellationToken);
      }
      return Unit.Value;
    }
  }
}
