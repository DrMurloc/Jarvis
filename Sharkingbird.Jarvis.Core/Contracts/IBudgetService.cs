using Sharkingbird.Jarvis.Core.Models.Discretionary;
using Sharkingbird.Jarvis.Core.Models.Vacation;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Contracts
{
  public interface IBudgetService
  {
    Task<DiscretionaryBudget> GetDiscretionaryBudget(CancellationToken cancellationToken);
    Task<VacationBudget> GetVacationBudget(CancellationToken cancellationToken);
  }
}
