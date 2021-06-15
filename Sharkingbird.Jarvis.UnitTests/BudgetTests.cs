using NUnit.Framework;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Core.Models.Enums;
using System;
using System.Linq;

namespace Sharkingbird.Jarvis.UnitTests
{
  public sealed class BudgetTests
  {
    [TestCase("RecurringTransaction",9.99,"Budget",RecurringRateEnum.Weekly)]
    public void Applying_Recurring_Transaction_To_Empty_Budget_Applies_Transaction(string recurringTransactionName,
      double amount, string budgetName, RecurringRateEnum rate)
    {
      var budget = new Budget(budgetName, new Transaction[0]);

      budget.ApplyRecurringTransactions(new[]
      {
        new RecurringTransaction(recurringTransactionName,budgetName,amount,rate)
      });

      Assert.AreEqual(amount, budget.Balance);
      Assert.AreEqual(1, budget.NewTransactions.Count());
    }

    [TestCase("RecurringTransaction", 9.99, "Budget", RecurringRateEnum.Weekly)]
    public void Applying_Recurring_Transaction_Twice_Only_Applies_Transaction_Once(string recurringTransactionName,
      double amount, string budgetName, RecurringRateEnum rate)
    {

      var budget = new Budget(budgetName, new Transaction[0]);
      var recurringTransaction = new RecurringTransaction(recurringTransactionName, budgetName, amount, rate);
      budget.ApplyRecurringTransactions(new[]
      {
        recurringTransaction,
        recurringTransaction
      });

      Assert.AreEqual(amount, budget.Balance);
      Assert.AreEqual(1,budget.NewTransactions.Count());
    }

    [TestCase("RecurringTransaction", 9.99, "Budget", RecurringRateEnum.Weekly)]
    public void Applying_Recurring_Transaction_When_Transaction_Exists_Does_Not_Apply_New_Transaction(string recurringTransactionName,
      double amount, string budgetName, RecurringRateEnum rate)
    {

      var budget = new Budget(budgetName, new[] {
        new Transaction(Guid.NewGuid(),amount,"",DateTimeOffset.Now,recurringTransactionName)
      });
      var recurringTransaction = new RecurringTransaction(recurringTransactionName, budgetName, amount, rate);
      budget.ApplyRecurringTransactions(new[] { recurringTransaction });


      Assert.AreEqual(amount, budget.Balance);
      Assert.AreEqual(0, budget.NewTransactions.Count());
    }

    [TestCase("RecurringTransaction", 9.99, "Budget")]
    public void Applying_Weekly_Transaction_8_Days_Ago_Applies_New_Transaction_Once(string recurringTransactionName,
      double amount, string budgetName)
    {
      var eightDaysAgo = DateTimeOffset.Now - TimeSpan.FromDays(8);

      var budget = new Budget(budgetName, new[] {
        new Transaction(Guid.NewGuid(),amount,"",eightDaysAgo,recurringTransactionName)
      });

      var recurringTransaction = new RecurringTransaction(recurringTransactionName, budgetName, amount, RecurringRateEnum.Weekly);
      budget.ApplyRecurringTransactions(new[] { recurringTransaction });


      Assert.AreEqual(2.0 * amount, budget.Balance);
      Assert.AreEqual(1, budget.NewTransactions.Count());
    }

    [TestCase("RecurringTransaction", 9.99, "Budget")]
    public void Applying_Weekly_Transaction_15_Days_Ago_Applies_New_Transaction_Twice(string recurringTransactionName,
      double amount, string budgetName)
    {
      var fifteenDaysAgo = DateTimeOffset.Now - TimeSpan.FromDays(15);

      var budget = new Budget(budgetName, new[] {
        new Transaction(Guid.NewGuid(),amount,"",fifteenDaysAgo,recurringTransactionName)
      });

      var recurringTransaction = new RecurringTransaction(recurringTransactionName, budgetName, amount, RecurringRateEnum.Weekly);
      budget.ApplyRecurringTransactions(new[] { recurringTransaction });


      Assert.AreEqual(3.0 * amount, budget.Balance);
      Assert.AreEqual(2, budget.NewTransactions.Count());
    }
  }
}
