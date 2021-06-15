using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MediatR;
using Sharkingbird.Jarvis.Core.Mediation.Queries;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class GetBudgetFunction
  {
    private readonly IMediator _mediator;
    public GetBudgetFunction(IMediator mediator)
    {
      _mediator = mediator;
    }
    [FunctionName("GetBudgetFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Budget/{budgetName}")] HttpRequest req,
        [FromRoute] string budgetName)
    {
      var budget = await _mediator.Send(new GetBudgetByNameQuery(budgetName));
      return new JsonResult(budget);
    }
  }
}
