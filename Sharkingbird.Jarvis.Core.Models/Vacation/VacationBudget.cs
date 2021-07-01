using System;
using MediatR;
using Sharkingbird.Jarvis.Core.Models.Events;
using System.Collections.Generic;
using System.Linq;
using Sharkingbird.Jarvis.Core.Models.Discretionary;
using Sharkingbird.Jarvis.Core.Models.Helpers;

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
    public Vacation GetVacationThatEndedYesterday()
    {
      var yesterday = DateTimeOffset.Now - TimeSpan.FromDays(1);
      return Vacations.FirstOrDefault(v => v.End.Year == yesterday.Year && v.End.Month == yesterday.Month && v.End.Day == yesterday.Day);
    }
    public Vacation CurrentVacation =>
      Vacations.FirstOrDefault(v => v.Start.StartOfDay() <= DateTimeOffset.Now && DateTimeOffset.Now <= v.End.EndOfDay());
    public void AddTransactionsToExpenses(IEnumerable<Transaction> transactions)
    {
      var transactionsArray = transactions.ToArray();
      if (!transactionsArray.Any())
      {
        return;
      }
      if (CurrentVacation == null)
      {
        _mediator.Publish(new VacationTransactionsAppliedOutsideOfVacationEvent(transactionsArray)).Wait();
      }
      else
      {
        CurrentVacation.AddTransactionsToExpenses(transactionsArray);
      }

      AdjustBalance(-transactionsArray.Sum(t => t.Amount));

    }
    public decimal MonthlyAllowance { get; }
    public void ApplyAllowance()
    {
      AdjustBalance(MonthlyAllowance);
    }

    public decimal GetProjectedBalance(DateTimeOffset projectionDate)
    {
      var upcomingExpenses = Vacations
        .Where(v => v.Start > DateTimeOffset.Now && v.Start <= projectionDate)
        .SelectMany(v=>v.Expenses)
        .Sum(e => e.Amount);
      var monthsDifference = projectionDate.Month - DateTimeOffset.Now.Month +
                             12 * (projectionDate.Year - DateTimeOffset.Now.Year);
      return Balance + monthsDifference * MonthlyAllowance - upcomingExpenses;
    }
  }
}
