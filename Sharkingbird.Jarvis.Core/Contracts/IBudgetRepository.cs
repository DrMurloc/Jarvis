using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Core.Models.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Contracts
{
  public interface IBudgetRepository
  {
    Task<Budget> GetBudget(BudgetNameValueType budgetName, CancellationToken cancellationToken);
    Task<IEnumerable<Budget>> GetBudgets(CancellationToken cancellationToken);
    Task SaveBudget(Budget budget, CancellationToken cancellationToken);
  }
}
