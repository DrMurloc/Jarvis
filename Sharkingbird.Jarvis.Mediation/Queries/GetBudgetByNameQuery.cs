using MediatR;
using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Mediation.Queries
{
  public sealed class GetBudgetByNameQuery : IRequest<Budget>
  {
    public GetBudgetByNameQuery(string budgetName)
    {
      BudgetName = budgetName;
    }
    public string BudgetName { get; set; }
  }
}
