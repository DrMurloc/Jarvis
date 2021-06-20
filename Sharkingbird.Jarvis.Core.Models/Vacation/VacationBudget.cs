using System;
using MediatR;
using Sharkingbird.Jarvis.Core.Models.Events;
using System.Collections.Generic;
using System.Linq;
using Sharkingbird.Jarvis.Core.Models.Discretionary;

namespace Sharkingbird.Jarvis.Core.Models.Vacation
{
  public sealed class VacationBudget
  {
    private readonly IMediator _mediator;
    public VacationBudget(IMediator mediator, decimal monthlyAllowance, decimal balance, IEnumerable<Vacation> vacations)
    {
      Vacations = vacations;
      MonthlyAllowance = monthlyAllowance;
      Balance = balance;
      _mediator = mediator;
    }
    public decimal Balance { get; private set; }

    private void AdjustBalance(decimal amount)
    {
      Balance += amount;
      _mediator.Publish(new VacationBudgetBalanceModifiedEvent(Balance));
    }
    public IEnumerable<Vacation> Vacations { get; }

    public Vacation CurrentVacation =>
      Vacations.FirstOrDefault(v => v.Start <= DateTimeOffset.Now && DateTimeOffset.Now <= v.End);
    public void AddTransactionsToExpenses(IEnumerable<Transaction> transactions)
    {
      var transactionsArray = transactions.ToArray();
      if (CurrentVacation == null)
      {
        _mediator.Publish(new VacationTransactionsAppliedOutsideOfVacationEvent(transactionsArray)).Wait();
      }
      else
      {
        CurrentVacation.AddTransactionsToExpenses(transactionsArray);
      }

      AdjustBalance(transactionsArray.Sum(t => t.Amount));

    }
    public decimal MonthlyAllowance { get; }
    public void ApplyAllowance()
    {
      AdjustBalance(MonthlyAllowance);
    }
  }
}
