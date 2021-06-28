using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class SendVacationReportHandler : IRequestHandler<SendVacationReportCommand>
  {
    private readonly IBudgetService _budgetService;
    private readonly INotificationService _notificationService;
    public SendVacationReportHandler(IBudgetService budgetService,
      INotificationService notificationService)
    {
      _budgetService = budgetService;
      _notificationService = notificationService;
    }
    public async Task<Unit> Handle(SendVacationReportCommand request, CancellationToken cancellationToken)
    {
      var budget = await _budgetService.GetVacationBudget(cancellationToken);
      var vacation = budget.GetVacationThatEndedYesterday();
      if (vacation == null)
      {
        return Unit.Value;
      }
      var dayCount = (int)Math.Ceiling((vacation.End - vacation.Start).TotalDays);
      var total = vacation.GetTotalExpenses(false);
      var average = total / dayCount;
      var message = $@"{vacation.Name}
{dayCount} Days
{total:0.00} Total
{average:0.00} Per Day";
      await _notificationService.PushNotification(new Notification(message), cancellationToken);
      return Unit.Value;
    }
  }
}
