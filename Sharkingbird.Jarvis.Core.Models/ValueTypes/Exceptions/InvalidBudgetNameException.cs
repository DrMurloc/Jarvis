using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Models.ValueTypes.Exceptions
{
  public sealed class InvalidBudgetNameException : Exception
  {
    public InvalidBudgetNameException(string reason) : base("Invalid Budget Name: "+reason)
    {

    }
  }
}
