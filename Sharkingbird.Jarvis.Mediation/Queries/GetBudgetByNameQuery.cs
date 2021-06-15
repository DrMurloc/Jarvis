using MediatR;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Core.Models.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Mediation.Queries
{
  public sealed class GetBudgetByNameQuery : IRequest<Budget>
  {
    public GetBudgetByNameQuery(BudgetNameValueType budgetName)
    {
      BudgetName = budgetName;
    }
    public BudgetNameValueType BudgetName { get; set; }
  }
}
