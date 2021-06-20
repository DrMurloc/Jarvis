using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class ImportNewTransactionsHandler : IRequestHandler<ImportNewTransactionsCommand>
  {
    private readonly IBudgetService _budgetService;
    private readonly ITransactionNotificationsRepository _transactionNotificationsRepository;

    public ImportNewTransactionsHandler(IBudgetService budgetService,
      ITransactionNotificationsRepository transactionNotificationsRepository)
    {
      _budgetService = budgetService;
      _transactionNotificationsRepository = transactionNotificationsRepository;
    }
    public async Task<Unit> Handle(ImportNewTransactionsCommand request, CancellationToken cancellationToken)
    {
      await ImportDiscretionaryTransactions(cancellationToken);
      await ImportVacationTransactions(cancellationToken);
      return Unit.Value;
    }
    private async Task ImportDiscretionaryTransactions(CancellationToken cancellationToken)
    {
      var budget = await _budgetService.GetDiscretionaryBudget(cancellationToken);
      var transactions = await _transactionNotificationsRepository.GetNewDiscretionaryTransactions(cancellationToken);
      budget.ApplyTransactions(transactions);
    }

    private async Task ImportVacationTransactions(CancellationToken cancellationToken)
    {
      var vacation = await _budgetService.GetVacationBudget(cancellationToken);
      var transactions = await _transactionNotificationsRepository.GetNewVacationTransactions(cancellationToken);
      vacation.AddTransactionsToExpenses(transactions);
    }
  }
}
