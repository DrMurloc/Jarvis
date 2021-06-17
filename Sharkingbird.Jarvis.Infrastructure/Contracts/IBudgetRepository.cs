using Sharkingbird.Jarvis.Infrastructure.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure.Contracts
{
  public interface IBudgetRepository
  {
    Task<BudgetEntity> GetBudgetByName(string name, CancellationToken cancellationToken);
    Task UpdateBudgetBalance(string name, decimal newBalance, CancellationToken cancellationToken);
  }
}
