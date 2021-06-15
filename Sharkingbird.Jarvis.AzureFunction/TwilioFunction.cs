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
using System.Text.RegularExpressions;
using Sharkingbird.Jarvis.Core.Mediation.Commands;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class TwilioFunction
  {
    private readonly IMediator _mediator;
    Regex _messageParser = new Regex(@"(\$|)([0-9\.]*)\s*(.*)", RegexOptions.Compiled);
    public TwilioFunction(IMediator mediator)
    {
      _mediator = mediator;
    }
    [FunctionName("TwilioFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Twilio")] HttpRequest request,
        ILogger log)
    {
      var message = request.Form["Body"];
      var parse = _messageParser.Match(message);
      var amount = decimal.Parse(parse.Groups[2].Value);
      var description = parse.Groups[3].Value.Trim();
      await _mediator.Send(new ApplyTransactionCommand("BlueCard", description, amount));
      return new OkResult();
    }
  }
}
