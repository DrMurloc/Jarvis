using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Sharkingbird.Jarvis.Core.Mediation.Commands;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class ImportVacationsFunction
  {
    private readonly IMediator _mediator;
    public ImportVacationsFunction(IMediator mediator)
    {
      _mediator = mediator;
    }
    [FunctionName("ImportVacationsFunction")]
    public async Task Run([TimerTrigger("0 0 11/2 * * *")]TimerInfo myTimer, ILogger log)
    {
      await _mediator.Send(new ImportVacationsCommand());
    }
  }
}
