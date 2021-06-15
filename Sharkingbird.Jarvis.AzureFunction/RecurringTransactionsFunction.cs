using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Sharkingbird.Jarvis.Core.Mediation.Commands;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class RecurringTransactionsFunction
  {
    private readonly IMediator _mediator;
    public RecurringTransactionsFunction(IMediator mediator)
    {
      _mediator = mediator;
    }
    [FunctionName("RecurringTransactionsFunction")]
    public async Task Run([TimerTrigger("0 0 9 * * *")]TimerInfo myTimer, ILogger log)
    {
      await _mediator.Send(new ApplyRecurringTransactionsCommand());
    }
  }
}
