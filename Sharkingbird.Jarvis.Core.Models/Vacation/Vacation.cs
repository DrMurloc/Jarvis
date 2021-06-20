using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Sharkingbird.Jarvis.Core.Models.Discretionary;
using Sharkingbird.Jarvis.Core.Models.Events;

namespace Sharkingbird.Jarvis.Core.Models.Vacation
{
  public sealed class Vacation
  {
    private readonly IMediator _mediator;
    public Vacation(IMediator mediator, Guid id, string name, DateTimeOffset start,
      DateTimeOffset end, IEnumerable<VacationExpense> expenses)
    {
      Id = id;
      Name = name;
      Start = start;
      End = end;
      _expenses = expenses.ToList();
      _mediator = mediator;
    }
    public Guid Id { get; }
    public string Name { get; }
    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }
    private IList<VacationExpense> _expenses;
    public IEnumerable<VacationExpense> Expenses => _expenses;

    public void AddExpenses(IEnumerable<VacationExpense> vacationExpenses)
    {
      foreach (var expense in vacationExpenses)
      {
        var existingExpense = _expenses.FirstOrDefault(e =>
          e.IsPaid == expense.IsPaid && e.Name.Equals(expense.Name, StringComparison.OrdinalIgnoreCase));

        if (existingExpense == null)
        {
          _expenses.Add(expense);
          continue;
        }

        existingExpense.Amount += expense.Amount;

      }

      _mediator.Publish(new VacationExpensesModifiedEvent(this));
    }
    public void AddTransactionsToExpenses(IEnumerable<Transaction> transactions)
    {
      var expenses = transactions.Select(t => new VacationExpense(t.Description, true, t.Amount));
      AddExpenses(expenses);
    }
    public override string ToString()
    {
      return Name + " - " + Start.ToString("d");
    }
  }
}
