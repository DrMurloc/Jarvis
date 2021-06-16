using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Contracts
{
  public interface ICalendarEventRepository
  {
    Task<IEnumerable<Vacation>> GetUpcomingCalanderEvents(CancellationToken cancellationToken);
  }
}
