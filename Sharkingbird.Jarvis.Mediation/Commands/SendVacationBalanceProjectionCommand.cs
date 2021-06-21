using System;
using MediatR;

namespace Sharkingbird.Jarvis.Core.Mediation.Commands
{
  public sealed class SendVacationBalanceProjectionCommand : IRequest
  {
    public SendVacationBalanceProjectionCommand(DateTimeOffset projectionDate)
    {
      ProjectionDate = projectionDate;
    }
    public DateTimeOffset ProjectionDate { get; }
  }
}
