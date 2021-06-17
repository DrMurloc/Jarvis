using System;
using System.Collections.Generic;

namespace Sharkingbird.Jarvis.Core.Models.Vacation
{
  public sealed class Vacation
  {
    public Vacation(Guid id, string name, DateTimeOffset start,
      DateTimeOffset end, IEnumerable<VacationExpense> expenses)
    {
      Id = id;
      Name = name;
      Start = start;
      End = end;
      Expenses = expenses;
    }
    public Guid Id { get; }
    public string Name { get; }
    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }
    public IEnumerable<VacationExpense> Expenses { get; }
    public override string ToString()
    {
      return Name + " - " + Start.ToString("d");
    }
  }
}
