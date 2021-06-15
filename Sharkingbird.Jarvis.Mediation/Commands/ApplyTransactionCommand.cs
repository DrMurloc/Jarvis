using MediatR;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Core.Models.ValueTypes;
using System;

namespace Sharkingbird.Jarvis.Core.Mediation.Commands
{
  public sealed class ApplyTransactionCommand : IRequest
  {
    public ApplyTransactionCommand(BudgetNameValueType budgetName, string description, decimal amount)
    {
      BudgetName = budgetName;
      Description = description ?? throw new ArgumentNullException(nameof(description));
      Amount = amount;
    }
    public BudgetNameValueType BudgetName { get; }
    public string Description { get; }
    public decimal Amount { get; }

  }
}
