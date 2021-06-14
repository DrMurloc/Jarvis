using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using Sharkingbird.Jarvis.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class ProcessRecurringPaymentsHandler : IRequestHandler<ProcessRecurringPaymentsCommand>
  {
    private readonly INotificationService _notificationService;
    private readonly IEnumerable<IRecurringPaymentRepository> _recurringPaymentRepositories;
    public ProcessRecurringPaymentsHandler(INotificationService notificationService,
      IEnumerable<IRecurringPaymentRepository> recurringPaymentRepositories)
    {
      _notificationService = notificationService;
      _recurringPaymentRepositories = recurringPaymentRepositories;
    }
    public async Task<Unit> Handle(ProcessRecurringPaymentsCommand request, CancellationToken cancellationToken)
    {
      var payments = new List<RecurringPayment>();
      foreach(var repo in _recurringPaymentRepositories)
      {
        payments.AddRange(await repo.GetNewRecurringPayments(cancellationToken));
      }
      var messages = payments.Select(p => new Notification($@"Payment to {p.SourceName}: ${p.Amount} scheduled for {p.ScheduledTime.ToString("d")}"));
      await _notificationService.PushNotifications(messages,cancellationToken);
      return Unit.Value;
    }
  }
}
