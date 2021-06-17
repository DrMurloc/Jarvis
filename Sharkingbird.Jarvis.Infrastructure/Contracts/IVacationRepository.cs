using Sharkingbird.Jarvis.Core.Models.Vacation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure.Contracts
{
  public interface IVacationRepository
  {
    Task<IEnumerable<Vacation>> GetUpcomingCalanderEvents(CancellationToken cancellationToken);
  }
}
