using MediatR;
using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Mediation.Events
{
  public sealed class TransactionsAppliedEvent : INotification
  {
    public TransactionsAppliedEvent(Budget budget)
    {
      Budget = budget ?? throw new ArgumentNullException(nameof(budget));
    }
    public Budget Budget { get; }
  }
}
