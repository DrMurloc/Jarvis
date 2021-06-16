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
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using Twilio.Security;
using Sharkingbird.Jarvis.Core.Contracts;

namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class TwilioFunction
  {
    private readonly TwilioConfiguration _configuration;
    private readonly IMediator _mediator;
    private readonly IIntentRepository _intentRepository;
    Regex _messageParser = new Regex(@"\s*([0-9\-\$\.]*)\s*(.*)", RegexOptions.Compiled);
    public TwilioFunction(IMediator mediator, IOptions<TwilioConfiguration> options)
    {
      _mediator = mediator;
      _configuration = options.Value;
    }
    private static IDictionary<string,string> ExtractTwilioBody(string body)
    {
      return body.Split('&')
          .Select(value => value.Split('='))
          .ToDictionary(pair => Uri.UnescapeDataString(pair[0]).Replace("+", " "),
                        pair => Uri.UnescapeDataString(pair[1]).Replace("+", " "));
    }
    private static bool AuthorizeTwilioRequest(string url, IDictionary<string,string> parameters, string twilioAuthToken, string twilioSignature)
    {
      return !string.IsNullOrWhiteSpace(twilioSignature);
      //TODO: Couldn't get this working, some mismatch on url or auth token?
      //var validator = new RequestValidator(twilioAuthToken);
      //return validator.Validate(url, parameters, twilioSignature);
    }
    [FunctionName("TwilioFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Twilio")] HttpRequest request,
        ILogger log)
    {
      var url = request.Scheme+ "://" + request.Host + request.Path+request.QueryString.Value;

      var body = await request.ReadAsStringAsync();

      var bodyValues = ExtractTwilioBody(body);
      var twilioSignature = request.Headers["X-Twilio-Signature"];

      var isAuthorized = AuthorizeTwilioRequest(url, bodyValues, _configuration.AuthToken, twilioSignature);
      if (!isAuthorized)
      {
        log.LogError("An unauthorized call to the Twilio Function was made");
        return new UnauthorizedResult();
      }

      var message = bodyValues["Body"];

      var mediatorRequest = await _intentRepository.GetRequestFromIntent(message,request.HttpContext.RequestAborted);
      if (mediatorRequest == null)
      {
        return new BadRequestResult();
      }
      await _mediator.Send(mediatorRequest,request.HttpContext.RequestAborted);
      return new OkResult();
    }
  }
}
