using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sharkingbird.Jarvis.Core.Mediation.Commands;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class ImportTransactionsFunction
  {
    private readonly IMediator _mediator;
    public ImportTransactionsFunction(IMediator mediator)
    {
      _mediator = mediator;
    }
    [FunctionName("ImportTransactions")]
    public async Task Run([TimerTrigger("0 0 9 * * *")] TimerInfo myTimer, ILogger log)
    {
      await _mediator.Send(new ImportNewTransactionsCommand());
    }
  }
}
