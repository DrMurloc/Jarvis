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
  public sealed class TransactionRepository : ITransactionRepository,
    INotificationHandler<TransactionAppliedEvent>
  {
    private readonly JarvisDbContext _dbContext;
    public TransactionRepository(JarvisDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<IEnumerable<Transaction>> GetDiscretionaryTransactions(CancellationToken cancellationToken)
    {
      return await _dbContext.Transaction.Select(t => new Transaction(t.Id, t.Amount, t.Description, t.AppliedOn)).ToArrayAsync(cancellationToken);
    }

    public async Task Handle(TransactionAppliedEvent notification, CancellationToken cancellationToken)
    {
      var transaction = notification.Transaction;
      var newEntity = new TransactionEntity
      {
        Id = transaction.Id,
        Amount=transaction.Amount,
        AppliedOn = transaction.AppliedOn,
        Description = transaction.Description
      };
      await _dbContext.AddAsync(newEntity,cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);
    }
  }
}
