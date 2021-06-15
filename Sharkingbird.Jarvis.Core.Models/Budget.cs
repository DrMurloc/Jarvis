using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Models
{
  public sealed class Budget
  {
    public Budget(string name, double balance)
    {
      Name = name;
      Balance = balance;
    }
    public string Name { get;}
    public double Balance { get; }
  }
}
