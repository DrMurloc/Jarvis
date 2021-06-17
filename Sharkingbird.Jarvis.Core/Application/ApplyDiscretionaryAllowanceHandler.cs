using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class ApplyDiscretionaryAllowanceHandler : IRequestHandler<ApplyDiscretionaryAllowanceCommand>
  {
    private readonly IBudgetService _budgetService;
    public ApplyDiscretionaryAllowanceHandler(IBudgetService budgetService)
    {
      _budgetService = budgetService;
    }

    public async Task<Unit> Handle(ApplyDiscretionaryAllowanceCommand request, CancellationToken cancellationToken)
    {
      var budget = await _budgetService.GetDiscretionaryBudget(cancellationToken);
      budget.ApplyAllowance();
      return Unit.Value;
    }
  }
}
