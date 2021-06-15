using Microsoft.EntityFrameworkCore;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Core.Models.Enums;
using Sharkingbird.Jarvis.Core.Models.ValueTypes;
using Sharkingbird.Jarvis.Infrastructure.Entities;
using Sharkingbird.Jarvis.Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure
{
  public sealed class TransactionRepository : ITransactionRepository
  {
    private readonly JarvisDbContext _dbContext;
    public TransactionRepository(JarvisDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<IEnumerable<RecurringTransaction>> GetRecurringTransactions(CancellationToken cancellationToken)
    {
      return await (
        from r in _dbContext.RecurringTransaction
        join b in _dbContext.Budget on r.BudgetId equals b.Id
        select new RecurringTransaction(r.Name,b.Name,r.Amount,Enum.Parse<RecurringRateEnum>(r.Rate))).ToArrayAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByBudget(BudgetNameValueType budgetName, CancellationToken cancellationToken)
    {
      return await (
        from b in _dbContext.Budget
        join t in _dbContext.Transaction on b.Id equals t.BudgetId
        join r in _dbContext.RecurringTransaction on t.RecurringTransactionId equals r.Id into gj
        from subr in gj.DefaultIfEmpty()
        where b.Name == budgetName.ToString()
        select new Transaction(t.Id, t.Amount, t.Description, t.AppliedOn, subr.Name)).ToArrayAsync(cancellationToken);
    }

    public async Task SaveTransactions(BudgetNameValueType budgetName, IEnumerable<Transaction> transactions, CancellationToken cancellationToken)
    {
      var budgetId = (await _dbContext.Budget.FirstOrDefaultAsync(b => b.Name == budgetName.ToString(), cancellationToken)).Id;
      foreach(var transaction in transactions)
      {
        var entity = _dbContext.Transaction.FirstOrDefaultAsync(t => t.Id == transaction.Id);
        if (entity == null)
        {
          var recurringTransactionId = transaction.RecurringTransactionName == null ? null : (await _dbContext.RecurringTransaction.FirstOrDefaultAsync(t => t.Name == transaction.RecurringTransactionName.ToString(), cancellationToken))?.Id;
          await _dbContext.Transaction.AddAsync(new TransactionEntity
          {
            Id = transaction.Id,
            Amount = transaction.Amount,
            AppliedOn = transaction.AppliedOn,
            Description = transaction.Description,
            RecurringTransactionId = recurringTransactionId,
            BudgetId = budgetId
          },cancellationToken);
          await _dbContext.SaveChangesAsync(cancellationToken);
        }
      }
    }
  }
}
