using MediatR;
using System;

namespace Sharkingbird.Jarvis.Core.Mediation.Commands
{
  public sealed class ApplyTransactionCommand : IRequest
  {
    public ApplyTransactionCommand(string description, decimal amount)
    {
      Description = description ?? throw new ArgumentNullException(nameof(description));
      Amount = amount;
    }
    public string Description { get; }
    public decimal Amount { get; }

  }
}
