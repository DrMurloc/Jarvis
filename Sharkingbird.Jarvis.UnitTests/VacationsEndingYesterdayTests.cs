using FakeItEasy;
using MediatR;
using NUnit.Framework;
using Sharkingbird.Jarvis.Core.Models.Vacation;
using System;

namespace Sharkingbird.Jarvis.UnitTests
{
  public sealed class VacationsEndingYesterdayTests
  {
    [Test]
    public void GetVacationsEndingYesterdayPicksUpVacationEndingYesterday()
    {
      var budget = new VacationBudget(A.Fake<IMediator>(), 0, 0, new[]
      {

        new Vacation(A.Fake<IMediator>(),Guid.NewGuid(),"NotYesterdayVacation",DateTimeOffset.Now-TimeSpan.FromDays(10),DateTimeOffset.Now-TimeSpan.FromDays(3),new VacationExpense[0]),
        new Vacation(A.Fake<IMediator>(),Guid.NewGuid(),"YesterdayVacation",DateTimeOffset.Now-TimeSpan.FromDays(10),DateTimeOffset.Now-TimeSpan.FromDays(1)+TimeSpan.FromMinutes(1),new VacationExpense[0])
      });
      var actual = budget.GetVacationThatEndedYesterday();
      Assert.AreEqual("YesterdayVacation", actual.Name);

    }
  }
}
