using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Sharkingbird.Jarvis.Infrastructure.DTOs
{
  public sealed class LuisResponseDto
  {
    public IntentPredictionDto Prediction { get; set; }
    public sealed class IntentPredictionDto
    {
      public string TopIntent { get; set; }
      public EntityDto Entities { get; set; }
      public sealed class EntityDto
      {
        //"datetimeV2":[{"type":"date","values":[{"timex":"2022-06-21","resolution":[{"value":"2022-06-21"}]}]}]
        public DateTimeDto[] DatetimeV2 { get; set; }
        public TransactionDto[] Transaction { get; set; }

        public sealed class DateTimeDto
        {
          public string Type { get; set; }
          public DateTimeValueDto[] Values { get; set; }
          public sealed class DateTimeValueDto
          {
            public string Timex { get; set; }
          }
        }
        public sealed class TransactionDto
        {
          public string[] Amount { get; set; }
          public string[] Budget { get; set; }
          public string[] Description { get; set; }
        }
      }
    }
  }
}
