using Sharkingbird.Jarvis.Core;
using Sharkingbird.Jarvis.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Contracts
{
  public interface IRecurringPaymentRepository
  {
    Task<IEnumerable<RecurringPayment>> GetNewRecurringPayments(CancellationToken cancellationToken);
  }
}
