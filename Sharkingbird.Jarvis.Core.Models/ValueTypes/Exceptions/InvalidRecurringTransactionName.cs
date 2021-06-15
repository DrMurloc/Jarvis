using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Models.ValueTypes.Exceptions
{
  public sealed class InvalidRecurringTransactionName : Exception
  {
    public InvalidRecurringTransactionName(string reason) : base("Invalid Recurring Transaction Name: " + reason)
    {

    }
  }
}
