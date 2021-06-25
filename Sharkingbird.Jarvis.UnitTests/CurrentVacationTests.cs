using System;
using FakeItEasy;
using MediatR;
using NUnit.Framework;
using Sharkingbird.Jarvis.Core.Models.Vacation;

namespace Sharkingbird.Jarvis.UnitTests
{
  public sealed class CurrentVacationTests
  {
    [TestCase("Name", 10, 5)]
    public void TodayAfterEndDateReturnsNoCurrentVacation(string vacationName, int startDaysOffset,
      int endDaysOffset)
    {
      var startDate = DateTimeOffset.Now - TimeSpan.FromDays(startDaysOffset);
      var endDate = DateTimeOffset.Now - TimeSpan.FromDays(endDaysOffset);
      var vacation = new Vacation(A.Fake<IMediator>(), Guid.NewGuid(), vacationName, startDate, endDate,
        new VacationExpense[0]);
      var budget = new VacationBudget(A.Fake<IMediator>(), 0, 0, new[] { vacation });
      var currentVacation = budget.CurrentVacation;
      Assert.Null(currentVacation);
    }
    [TestCase("Name",5,10)]
    public void TodayBeforeStartDateReturnsNoCurrentVacation(string vacationName, int startDaysOffset,
      int endDaysOffset)
    {
      var startDate = DateTimeOffset.Now + TimeSpan.FromDays(startDaysOffset);
      var endDate = DateTimeOffset.Now + TimeSpan.FromDays(endDaysOffset);
      var vacation = new Vacation(A.Fake<IMediator>(), Guid.NewGuid(), vacationName, startDate, endDate,
        new VacationExpense[0]);
      var budget = new VacationBudget(A.Fake<IMediator>(), 0, 0, new[] { vacation });
      var currentVacation = budget.CurrentVacation;
      Assert.Null(currentVacation);
    }
    [TestCase("Name",5,10)]
    public void TodayDuringEndDateReturnsCurrentVacation(string vacationName, int startDaysOffset, int endSecondsOffset)
    {
      var startDate = DateTimeOffset.Now - TimeSpan.FromDays(startDaysOffset);
      var endDate = DateTimeOffset.Now - TimeSpan.FromSeconds(endSecondsOffset);
      var vacation = new Vacation(A.Fake<IMediator>(), Guid.NewGuid(), vacationName, startDate, endDate,
        new VacationExpense[0]);
      var budget = new VacationBudget(A.Fake<IMediator>(), 0, 0, new[] {vacation});
      var currentVacation = budget.CurrentVacation;
      Assert.NotNull(currentVacation);
      Assert.AreEqual(vacationName,currentVacation.Name);
    }

    [TestCase("Name", 10, 5)]
    public void TodayDuringStartDateReturnsCurrentVacation(string vacationName, int startSecondsOffset, int endDaysOffset)
    {
      var startDate = DateTimeOffset.Now + TimeSpan.FromSeconds(startSecondsOffset);
      var endDate = DateTimeOffset.Now + TimeSpan.FromDays(endDaysOffset);
      var vacation = new Vacation(A.Fake<IMediator>(), Guid.NewGuid(), vacationName, startDate, endDate,
        new VacationExpense[0]);
      var budget = new VacationBudget(A.Fake<IMediator>(), 0, 0, new[] { vacation });
      var currentVacation = budget.CurrentVacation;
      Assert.NotNull(currentVacation);
      Assert.AreEqual(vacationName, currentVacation.Name);
    }
  }
}
