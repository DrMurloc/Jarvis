using Microsoft.EntityFrameworkCore;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
using Sharkingbird.Jarvis.Infrastructure.Entities;
using Sharkingbird.Jarvis.Infrastructure.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure.Repositories
{
  public sealed class BudgetRepository : IBudgetRepository
  {
    private JarvisDbContext _dbContext;
    public BudgetRepository(JarvisDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<BudgetEntity> GetBudgetByName(string name, CancellationToken cancellationToken)
    {
      return await _dbContext.Budget.FirstOrDefaultAsync(b => b.Name == name,cancellationToken);
    }
   

    public async Task UpdateBudgetBalance(string name, decimal newBalance, CancellationToken cancellationToken)
    {
      var entity = await GetBudgetByName(name, cancellationToken);
      entity.Balance = newBalance;
      await _dbContext.SaveChangesAsync(cancellationToken);
    }
  }
}
