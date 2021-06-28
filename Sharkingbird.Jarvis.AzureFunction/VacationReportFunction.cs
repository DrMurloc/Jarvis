using MediatR;
using Microsoft.Azure.WebJobs;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class VacationReportFunction
  {
    private readonly IMediator _mediator;
    public VacationReportFunction(IMediator mediator)
    {
      _mediator = mediator;
    }
    [FunctionName("VacationReportFunction")]
    public async Task Run([TimerTrigger("0 45 9 * * *")] TimerInfo myTimer)
    {
      await _mediator.Send(new SendVacationReportCommand());
    }
  }
}
