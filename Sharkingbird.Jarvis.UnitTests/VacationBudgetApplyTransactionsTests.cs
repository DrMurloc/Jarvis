using FakeItEasy;
using MediatR;
using NUnit.Framework;
using Sharkingbird.Jarvis.Core.Models.Discretionary;
using Sharkingbird.Jarvis.Core.Models.Events;
using Sharkingbird.Jarvis.Core.Models.Vacation;
using System;
using System.Threading;

namespace Sharkingbird.Jarvis.UnitTests
{
  public sealed class VacationBudgetApplyTransactionsTests
  {
    [Test]
    public void NoTransactionNotificationsWhenNoTransactionsApplied()
    {
      var mediator = A.Fake<IMediator>();
      var budget = new VacationBudget(mediator, 0, 0, new Vacation[0]);
      budget.AddTransactionsToExpenses(new Transaction[0]);

      A.CallTo(() => mediator.Publish(A<VacationTransactionsAppliedOutsideOfVacationEvent>.Ignored, A<CancellationToken>.Ignored))
        .MustNotHaveHappened();

      A.CallTo(() => mediator.Publish(A<VacationBudgetBalanceModifiedEvent>.Ignored, A<CancellationToken>.Ignored))
        .MustNotHaveHappened();
    }

    [Test]
    public void TransactionNotificationsWhenTransationsAreApplied()
    {
      var mediator = A.Fake<IMediator>();
      var budget = new VacationBudget(mediator, 0, 0, new Vacation[0]);
      budget.AddTransactionsToExpenses(new []{
        new Transaction(Guid.NewGuid(),10,"Description",DateTimeOffset.Now)
      });

      A.CallTo(() => mediator.Publish(A<VacationTransactionsAppliedOutsideOfVacationEvent>.Ignored, A<CancellationToken>.Ignored))
        .MustHaveHappened();

      A.CallTo(() => mediator.Publish(A<VacationBudgetBalanceModifiedEvent>.Ignored, A<CancellationToken>.Ignored))
        .MustHaveHappened();
    }
  }
}
