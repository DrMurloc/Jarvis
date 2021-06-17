using MediatR;
using Sharkingbird.Jarvis.Core.Models.Events;
using System.Collections.Generic;

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
    public IEnumerable<Vacation> Vacations { get; }
    public decimal MonthlyAllowance { get; }
    public void ApplyAllowance()
    {
      Balance += MonthlyAllowance;
      _mediator.Publish(new VacationBudgetBalanceModifiedEvent(Balance)).Wait();
    }
  }
}
