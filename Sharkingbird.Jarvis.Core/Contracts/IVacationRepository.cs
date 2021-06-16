using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Contracts
{
  public interface IVacationRepository
  {
    Task<IEnumerable<Vacation>> GetVacations(CancellationToken cancellationToken);
    Task SaveVacations(IEnumerable<Vacation> vacations, CancellationToken cancellationToken);
  }
}
