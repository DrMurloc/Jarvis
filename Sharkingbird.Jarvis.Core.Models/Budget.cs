using Sharkingbird.Jarvis.Core.Models.Enums;
using Sharkingbird.Jarvis.Core.Models.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharkingbird.Jarvis.Core.Models
{
  public sealed class Budget
  {
    public Budget(BudgetNameValueType name, IEnumerable<Transaction> transactions)
    {
      Name = name;
      _transactions = transactions.ToList();
      _newTransactions = new List<Transaction>();
    }
    private List<Transaction> _transactions;
    private List<Transaction> _newTransactions;
    public IEnumerable<Transaction> NewTransactions => _newTransactions;
    public BudgetNameValueType Name { get;}
    public double Balance => _transactions.Sum(t => t.Amount);
    public void ApplyRecurringTransactions(IEnumerable<RecurringTransaction> recurringTransactions)
    {
      foreach(var recurringTransaction in recurringTransactions)
      {
        var timeSpan = recurringTransaction.Rate.GetTimeSpan();
        while (!_transactions.Any(t => t.RecurringTransactionName == recurringTransaction.Name && t.AppliedOn + timeSpan <= DateTimeOffset.Now))
        {

          var lastTransaction = _transactions.OrderByDescending(t => t.AppliedOn).FirstOrDefault(t => t.RecurringTransactionName == recurringTransaction.Name);
          var newDate = lastTransaction?.AppliedOn + timeSpan ?? DateTimeOffset.Now;

          var newTransaction = new Transaction(Guid.NewGuid(), recurringTransaction.Amount, newDate, recurringTransaction.Name);
          _newTransactions.Add(newTransaction);
          _transactions.Add(newTransaction);
        }
      }
    }

  }
}
