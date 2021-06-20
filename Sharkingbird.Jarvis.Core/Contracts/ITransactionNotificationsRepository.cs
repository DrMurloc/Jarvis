using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sharkingbird.Jarvis.Core.Models.Discretionary;

namespace Sharkingbird.Jarvis.Core.Contracts
{
  public interface ITransactionNotificationsRepository
  {
    Task<IEnumerable<Transaction>> GetNewDiscretionaryTransactions(CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetNewVacationTransactions(CancellationToken cancellationToken);
  }
}
