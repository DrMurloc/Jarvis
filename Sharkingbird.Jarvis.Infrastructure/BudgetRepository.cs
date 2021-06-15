using Microsoft.EntityFrameworkCore;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Infrastructure.Entities;
using Sharkingbird.Jarvis.Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure
{
  public sealed class BudgetRepository : IBudgetRepository
  {
    private readonly JarvisDbContext _dbContext;
    public BudgetRepository(JarvisDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<Budget> GetBudget(string budgetName, CancellationToken cancellationToken)
    {
      var budget = await _dbContext.Budget.FirstOrDefaultAsync(b => b.Name == budgetName);
      if (budget == null)
      {
        budget = new BudgetEntity
        {
          Balance = 0,
          Name = budgetName
        };
      }

      return new Budget(budget.Name, budget.Balance);
    }
  }
}
