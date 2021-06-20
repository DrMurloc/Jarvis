using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Sharkingbird.Jarvis.Core.Models.Discretionary;

namespace Sharkingbird.Jarvis.Core.Models.Events
{
  public sealed class VacationTransactionsAppliedOutsideOfVacationEvent : INotification
  {
    public VacationTransactionsAppliedOutsideOfVacationEvent(IEnumerable<Transaction> transactions)
    {
      Transactions = transactions ?? throw new ArgumentNullException(nameof(transactions));
    }
    public IEnumerable<Transaction> Transactions { get; }
  }
}
