using System;

namespace Sharkingbird.Jarvis.Core.Models
{
  public sealed class RecurringPayment
  {
    public RecurringPayment(string sourceNameParam, DateTimeOffset scheduledTimeParam, double amountParam)
    {
      SourceName = sourceNameParam;
      Amount = amountParam;
      ScheduledTime = scheduledTimeParam;
    }
    public string SourceName { get; set; }
    public DateTimeOffset ScheduledTime { get; }
    public double Amount { get; }
  }
}
