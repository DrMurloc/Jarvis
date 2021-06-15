using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Contracts
{
  public interface IBudgetRepository
  {
    Task<Budget> GetBudget(string budgetName, CancellationToken cancellationToken);
  }
}
