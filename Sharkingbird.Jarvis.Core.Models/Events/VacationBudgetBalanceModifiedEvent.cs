using MediatR;

namespace Sharkingbird.Jarvis.Core.Models.Events
{
  public sealed class VacationBudgetBalanceModifiedEvent : INotification
  {
    public VacationBudgetBalanceModifiedEvent(decimal newBalance)
    {
      NewBalance = newBalance;
    }
    public decimal NewBalance { get; }
  }
}
