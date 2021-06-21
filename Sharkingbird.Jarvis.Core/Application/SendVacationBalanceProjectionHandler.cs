using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using Sharkingbird.Jarvis.Core.Models;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class SendVacationBalanceProjectionHandler : IRequestHandler<SendVacationBalanceProjectionCommand>
  {
    private readonly INotificationService _notificationService;
    private readonly IBudgetService _budgetService;

    public SendVacationBalanceProjectionHandler(INotificationService notificationService,
      IBudgetService budgetService)
    {
      _budgetService = budgetService;
      _notificationService = notificationService;
    }
    public async Task<Unit> Handle(SendVacationBalanceProjectionCommand request, CancellationToken cancellationToken)
    {
      var budget = await _budgetService.GetVacationBudget(cancellationToken);
      var projection = budget.GetProjectedBalance(request.ProjectionDate);
      await _notificationService.PushNotification(
        new Notification($"Vacation Balance Projection for {request.ProjectionDate:d}: ${projection}"),
        cancellationToken);
      return Unit.Value;
    }
  }
}
