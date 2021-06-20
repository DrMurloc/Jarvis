using System;
using MediatR;

namespace Sharkingbird.Jarvis.Core.Models.Events
{
  public sealed class VacationExpensesModifiedEvent : INotification
  {
    public VacationExpensesModifiedEvent(Vacation.Vacation vacation)
    {
      Vacation = vacation ?? throw new ArgumentNullException(nameof(vacation));
    }
    public Vacation.Vacation Vacation { get; }
  }
}
