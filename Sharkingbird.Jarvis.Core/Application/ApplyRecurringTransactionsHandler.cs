using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class ApplyRecurringTransactionsHandler : IRequestHandler<ApplyRecurringTransactionsCommand>
  {
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;
    public ApplyRecurringTransactionsHandler(ITransactionRepository transactionRepository,
      IBudgetRepository budgetRepository)
    {
      _transactionRepository = transactionRepository;
      _budgetRepository = budgetRepository;
    }
    public async Task<Unit> Handle(ApplyRecurringTransactionsCommand request, CancellationToken cancellationToken)
    {
      var recurringPayments = (await _transactionRepository.GetRecurringTransactions(cancellationToken)).ToArray();
      var budgets = await _budgetRepository.GetBudgets(cancellationToken);
      foreach(var budget in budgets)
      {
        budget.ApplyRecurringTransactions(recurringPayments);
        await _budgetRepository.SaveBudget(budget,cancellationToken);
      }
      return Unit.Value;
    }
  }
}
