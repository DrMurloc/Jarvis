using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using Sharkingbird.Jarvis.Core.Models.Discretionary;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class ApplyTransactionHandler : IRequestHandler<ApplyTransactionCommand>
  {
    private readonly IBudgetService _budgetRepository;
    private readonly IMediator _mediator;
    public ApplyTransactionHandler(IBudgetService budgetRepository,
      IMediator mediator)
    {
      _budgetRepository = budgetRepository;
      _mediator = mediator;
    }
    public async Task<Unit> Handle(ApplyTransactionCommand request, CancellationToken cancellationToken)
    {
      var budget = await _budgetRepository.GetDiscretionaryBudget(cancellationToken);
      budget.ApplyTransaction(new Transaction(Guid.NewGuid(), request.Amount, request.Description, DateTimeOffset.Now));

      return Unit.Value;
    }
  }
}
