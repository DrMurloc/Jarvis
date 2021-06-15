using Sharkingbird.Jarvis.Core.Models.Enums;
using Sharkingbird.Jarvis.Core.Models.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Models
{
  public sealed class RecurringTransaction
  {
    public RecurringTransaction(RecurringTransactionNameValueType name,
      BudgetNameValueType budgetName,
      double amount,
      RecurringRateEnum rate)
    {
      Name = name;
      Amount = amount;
      Rate = rate;
      BudgetName = budgetName;
    }
    public RecurringTransactionNameValueType Name { get; }
    public BudgetNameValueType BudgetName { get; }
    public double Amount { get; }
    
    public RecurringRateEnum Rate { get; }
  }
}
