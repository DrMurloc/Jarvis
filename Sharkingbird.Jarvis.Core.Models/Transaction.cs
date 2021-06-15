using Sharkingbird.Jarvis.Core.Models.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Models
{
  public sealed class Transaction
  {
    public Transaction(Guid id, decimal amount, string description, DateTimeOffset appliedOn, RecurringTransactionNameValueType? recurringTransactionName)
    {
      Id = id;
      Amount = amount;
      Description = description;
      RecurringTransactionName = recurringTransactionName;
      AppliedOn = appliedOn;
    }
    public Guid Id { get; }
    public RecurringTransactionNameValueType? RecurringTransactionName { get; }
    public decimal Amount { get; }
    public string Description { get; }
    public DateTimeOffset AppliedOn { get; }
  }
}
