using System;

namespace Sharkingbird.Jarvis.Core.Models.Discretionary
{
  public sealed class Transaction
  {
    public Transaction(Guid id, decimal amount, string description, DateTimeOffset appliedOn)
    {
      Id = id;
      Amount = amount;
      Description = description;
      AppliedOn = appliedOn;
    }
    public Guid Id { get; }
    public decimal Amount { get; }
    public string Description { get; }
    public DateTimeOffset AppliedOn { get; }
  }
}
