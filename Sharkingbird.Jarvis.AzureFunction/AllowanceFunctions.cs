using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sharkingbird.Jarvis.Core.Mediation.Commands;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class AllowanceFunctions
  {
    private readonly IMediator _mediator;
    public AllowanceFunctions(IMediator mediator)
    {
      _mediator = mediator;
    }
    [FunctionName("DiscretionaryAllowanceFunction")]
    public async Task RunDiscretionary([TimerTrigger("0 0 0 * * 5")]TimerInfo myTimer, ILogger log)
    {
      await _mediator.Send(new ApplyDiscretionaryAllowanceCommand());
    }
    [FunctionName("VacationBudgetAllowanceFunction")]
    public async Task RunVacation([TimerTrigger("0 0 0 1 * *")] TimerInfo myTimer, ILogger log)
    {
      await _mediator.Send(new ApplyVacationAllowanceCommand());
    }
  }
}
