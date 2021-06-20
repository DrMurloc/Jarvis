using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class ApplyVacationAllowanceHandler : IRequestHandler<ApplyVacationAllowanceCommand>
  {
    private readonly IBudgetService _budgetService;
    public ApplyVacationAllowanceHandler(IBudgetService budgetService)
    {
      _budgetService = budgetService;
    }
    public async Task<Unit> Handle(ApplyVacationAllowanceCommand request, CancellationToken cancellationToken)
    {
      var budget = await _budgetService.GetVacationBudget(cancellationToken);
      budget.ApplyAllowance();
      return Unit.Value;
    }
  }
}
