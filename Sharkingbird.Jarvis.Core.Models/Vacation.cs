using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Models
{
  public sealed class Vacation
  {
    public Vacation(Guid id, string name, DateTimeOffset start,
      DateTimeOffset end)
    {
      Id = id;
      Name = name;
      Start = start;
      End = end;
    }
    public Guid Id { get; }
    public string Name { get; }
    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }
    public override string ToString()
    {
      return Name + " - " + Start.ToString("d");
    }
  }
}
