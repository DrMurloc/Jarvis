using Microsoft.EntityFrameworkCore;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Core.Models.ValueTypes;
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
    private readonly ITransactionRepository _transactionRepository;
    public BudgetRepository(JarvisDbContext dbContext, ITransactionRepository transactionRepository)
    {
      _dbContext = dbContext;
      _transactionRepository = transactionRepository;
    }
    public async Task<Budget> GetBudget(BudgetNameValueType budgetName, CancellationToken cancellationToken)
    {
      var budget = await _dbContext.Budget.FirstOrDefaultAsync(b => b.Name == budgetName.ToString(),cancellationToken);
      if (budget == null)
      {
        budget = new BudgetEntity
        {
          Name = budgetName
        };
      }
      var transactions = await _transactionRepository.GetTransactionsByBudget(budgetName, cancellationToken);
      return new Budget(budget.Name, transactions);
    }


    public async Task<IEnumerable<Budget>> GetBudgets(CancellationToken cancellationToken)
    {
      var budgets = await _dbContext.Budget.ToArrayAsync(cancellationToken);
      var results = new List<Budget>();
      foreach(var budget in budgets)
      {
        var transactions = await _transactionRepository.GetTransactionsByBudget(budget.Name, cancellationToken);
        results.Add(new Budget(budget.Name, transactions));
      }

      return results;
    }

    public async Task SaveBudget(Budget budget, CancellationToken cancellationToken)
    {
      if (!budget.NewTransactions.Any())
      {
        return;
      }
      await _transactionRepository.SaveTransactions(budget.Name,budget.NewTransactions, cancellationToken);
    }
  }
}
