using MediatR;

namespace Sharkingbird.Jarvis.Core.Models.Events
{
  public sealed class DiscretionaryBudgetBalanceModifiedEvent : INotification
  {
    public DiscretionaryBudgetBalanceModifiedEvent(decimal newBalance)
    {
      NewBalance = newBalance;
    }
    public decimal NewBalance { get; }
  }
}
