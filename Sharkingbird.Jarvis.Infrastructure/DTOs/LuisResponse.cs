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
        public TransactionDto[] Transaction { get; set; }

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
