using Sharkingbird.Jarvis.Core.Models.Discretionary;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure.Contracts
{
  public interface ITransactionRepository
  {
    Task<IEnumerable<Transaction>> GetDiscretionaryTransactions(CancellationToken cancellationToken);
  }
}
