using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sharkingbird.Jarvis.Core.Mediation.Commands;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class RecurringPaymentFunction
  {
    private readonly IMediator _mediator;
    public RecurringPaymentFunction(IMediator mediator)
    {
      _mediator = mediator;
    }
    [FunctionName("RecurringPayments")]
    public async Task Run([TimerTrigger("0 0 9 * * *")]TimerInfo myTimer, ILogger log)
    {
      await _mediator.Send(new ProcessRecurringPaymentsCommand());
    }
  }
}
