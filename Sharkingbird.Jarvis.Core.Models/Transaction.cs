using Sharkingbird.Jarvis.Core.Models.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Models
{
  public sealed class Transaction
  {
    public Transaction(Guid id, double amount, DateTimeOffset appliedOn, RecurringTransactionNameValueType? recurringTransactionName)
    {
      Id = id;
      Amount = amount;
      RecurringTransactionName = recurringTransactionName;
      AppliedOn = appliedOn;
    }
    public Guid Id { get; }
    public RecurringTransactionNameValueType? RecurringTransactionName { get; }
    public double Amount { get; }
    public DateTimeOffset AppliedOn { get; }
  }
}
