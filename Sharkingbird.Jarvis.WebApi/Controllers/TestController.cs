using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.WebApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class TestController : Controller
  {
    [HttpGet]
    public async Task<IActionResult> GetTest([FromServices] IMediator mediator)
    {
      await mediator.Send(new ProcessRecurringPaymentsCommand(),Request.HttpContext.RequestAborted);
      return Ok();

    }
  }
}
