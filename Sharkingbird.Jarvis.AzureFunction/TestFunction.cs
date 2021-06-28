using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class TestFunction
  {
    private readonly IMediator _mediator;
    private readonly IIntentRepository _repo;
    public TestFunction(IMediator mediator,
      IIntentRepository repo)
    {
      _mediator = mediator;
      _repo = repo;
    }
    [FunctionName("TestFunction")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Test")] HttpRequest req)
    {
      return new JsonResult(new { });
    }
  }
}
