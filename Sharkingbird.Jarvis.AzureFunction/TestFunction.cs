using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using MediatR;
using Sharkingbird.Jarvis.Core.Mediation.Commands;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class TestFunction
  {
    private readonly IMediator _mediator;
    public TestFunction(IMediator mediator)
    {
      _mediator = mediator;
    }
    [FunctionName("TestFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Test")] HttpRequest req)
    {

      return new JsonResult(new { });
    }
  }
}
