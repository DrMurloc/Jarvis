using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharkingbird.Jarvis.Core.Models.Discretionary;
using Sharkingbird.Jarvis.Core.Models.Events;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
using Sharkingbird.Jarvis.Infrastructure.Entities;
using Sharkingbird.Jarvis.Infrastructure.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure.Repositories
{
  public sealed class SqlTransactionRepository : ITransactionRepository,
    INotificationHandler<TransactionsAppliedEvent>
  {
    private readonly JarvisDbContext _dbContext;
    public SqlTransactionRepository(JarvisDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<IEnumerable<Transaction>> GetDiscretionaryTransactions(CancellationToken cancellationToken)
    {
      return await _dbContext.Transaction.Select(t => new Transaction(t.Id, t.Amount, t.Description, t.AppliedOn)).ToArrayAsync(cancellationToken);
    }

    public async Task Handle(TransactionsAppliedEvent notification, CancellationToken cancellationToken)
    {

      var newEntities = notification.Transactions.Select(transaction => new TransactionEntity
      {
        Id = transaction.Id,
        Amount = transaction.Amount,
        AppliedOn = transaction.AppliedOn,
        Description = transaction.Description
      });
      await _dbContext.AddRangeAsync(newEntities,cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);
    }
  }
}
