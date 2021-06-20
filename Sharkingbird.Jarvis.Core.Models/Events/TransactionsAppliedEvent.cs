using MediatR;
using Sharkingbird.Jarvis.Core.Models.Discretionary;
using System;
using System.Collections.Generic;

namespace Sharkingbird.Jarvis.Core.Models.Events
{
  public sealed class TransactionsAppliedEvent : INotification
  {
    public TransactionsAppliedEvent(IEnumerable<Transaction> transactions)
    {
      Transactions = transactions ?? throw new ArgumentNullException(nameof(transactions));
    }
    public IEnumerable<Transaction> Transactions { get; }
  }
}
