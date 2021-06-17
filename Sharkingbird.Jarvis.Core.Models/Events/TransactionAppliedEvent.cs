using MediatR;
using Sharkingbird.Jarvis.Core.Models.Discretionary;
using System;

namespace Sharkingbird.Jarvis.Core.Models.Events
{
  public sealed class TransactionAppliedEvent : INotification
  {
    public TransactionAppliedEvent(Transaction transaction)
    {
      Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
    }
    public Transaction Transaction { get; }
  }
}
