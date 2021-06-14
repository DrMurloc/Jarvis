using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.WebApi.HostedServices
{
  [DisallowConcurrentExecution]
  public sealed class RecurringPaymentJob : IJob
  {
    private readonly IMediator _mediator;
    private readonly ILogger<RecurringPaymentJob> _logger;
    public RecurringPaymentJob(IMediator mediator,
      ILogger<RecurringPaymentJob> logger)
    {
      _mediator = mediator;
      _logger = logger;
    }
    public async Task Execute(IJobExecutionContext context)
    {
      _logger.LogInformation("Starting recurring payment job");
      await _mediator.Send(new ProcessRecurringPaymentsCommand(), context.CancellationToken);
    }
  }
}
