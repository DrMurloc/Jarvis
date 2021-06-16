namespace Sharkingbird.Jarvis.Infrastructure.DTOs
{
  internal sealed class LuisResponseDto
  {
    internal IntentPredictionDto Prediction { get; set; }
    internal EntityDto Entities { get; set; }
    internal sealed class EntityDto
    {
      internal TransactionDto[] Transaction { get; set; }

      internal sealed class TransactionDto
      {
        internal string[] Amount { get; set; }
        internal string[] Budget { get; set; }
        internal string[] Description { get; set; }
      }
    }
    internal sealed class IntentPredictionDto
    {
      internal string TopIntent { get; set; }
    }
  }
}
