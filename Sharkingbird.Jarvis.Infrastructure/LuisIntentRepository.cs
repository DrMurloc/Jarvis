using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using Sharkingbird.Jarvis.Infrastructure.DTOs;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure
{
  public sealed class LuisIntentRepository : IIntentRepository
  {
    private readonly HttpClient _client;
    private readonly LuisConfiguration _configuration;
    public LuisIntentRepository(IOptions<LuisConfiguration> options, IHttpClientFactory clientFactory)
    {
      _client = clientFactory.CreateClient("Luis");
      _configuration = options.Value;
    }
    public async Task<IRequest> GetRequestFromIntent(string request, CancellationToken cancellationToken)
    {
      var result = await _client.GetAsync($"https://westus.api.cognitive.microsoft.com/luis/prediction/v3.0/apps/57efed02-3129-47c2-bd4e-c3b5df3761f0/slots/production/predict?subscription-key={_configuration.SubscriptionKey}&show-all-intents=true&log=true&query={Uri.EscapeDataString(request)}",cancellationToken);
      result.EnsureSuccessStatusCode();

      var body = await result.Content.ReadAsStringAsync();
      var dto = JsonConvert.DeserializeObject<LuisResponseDto>(body);

      return dto.Prediction.TopIntent.ToLower() switch
      {
        "post transaction" => GetApplyTransactionCommand(dto),
        "send projection" => GetSendProjectionCommand(dto),
        _ => null
      };
    }

    private static SendVacationBalanceProjectionCommand GetSendProjectionCommand(LuisResponseDto dto)
    {
      var dateTime = dto.Prediction.Entities.DatetimeV2.First().Values.First().Timex;
      var predictionDate = DateTimeOffset.Parse(dateTime);
      return new SendVacationBalanceProjectionCommand(predictionDate);
    }
    private static ApplyTransactionCommand GetApplyTransactionCommand(LuisResponseDto dto)
    {
      var transaction = dto.Prediction.Entities.Transaction.First();
      var amount = decimal.Parse(transaction.Amount.First());
      var description = transaction.Description.First();
      return new ApplyTransactionCommand(description, amount);
    }
  }
}
