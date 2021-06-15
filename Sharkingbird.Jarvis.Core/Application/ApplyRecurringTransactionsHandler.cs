using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using Sharkingbird.Jarvis.Core.Mediation.Events;
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
    private readonly IMediator _mediator;
    public ApplyRecurringTransactionsHandler(ITransactionRepository transactionRepository,
      IBudgetRepository budgetRepository,
      IMediator mediator)
    {
      _transactionRepository = transactionRepository;
      _budgetRepository = budgetRepository;
      _mediator = mediator;
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
        await _mediator.Send(new TransactionsAppliedEvent(budget), cancellationToken);
      }
      return Unit.Value;
    }
  }
}
