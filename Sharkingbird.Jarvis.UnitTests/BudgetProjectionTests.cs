using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using MediatR;
using NUnit.Framework;
using Sharkingbird.Jarvis.Core.Models.Vacation;

namespace Sharkingbird.Jarvis.UnitTests
{
  public sealed class BudgetProjectionTests
  {
    [TestCase(2800,0,3)]
    [TestCase(300, 0, 0)]
    [TestCase(1600, 0, 13)]
    public void ProjectedAddsMonthlyAllowanceForEachMonthPassed(decimal monthlyAllowance, decimal balance, int monthsPassed)
    {
      var projectionDate = DateTimeOffset.Now.AddMonths(monthsPassed);
      var budget = new VacationBudget(A.Fake<IMediator>(), monthlyAllowance, balance, new Vacation[0]);

      var expected = balance + monthlyAllowance * monthsPassed;
      var actual = budget.GetProjectedBalance(projectionDate);
      Assert.AreEqual(expected,actual);
    }

    [TestCase(0,10,5,50)]
    public void ProjectionIgnoresOlderVacations(decimal balance, int projectionDaysInFuture, int daysOld, decimal amount)
    {
      var projectionDate = DateTimeOffset.Now + TimeSpan.FromDays(projectionDaysInFuture);
      var expense = new VacationExpense("Expense", true, amount);
      var vacation = new Vacation(A.Fake<IMediator>(), Guid.NewGuid(), "Vacation",
        DateTimeOffset.Now - TimeSpan.FromDays(daysOld), projectionDate + TimeSpan.FromDays(1), new[] {expense});
      var budget = new VacationBudget(A.Fake<IMediator>(), 0, balance, new[] {vacation});

      Assert.AreEqual(balance,budget.GetProjectedBalance(projectionDate));
    }
  }
}
