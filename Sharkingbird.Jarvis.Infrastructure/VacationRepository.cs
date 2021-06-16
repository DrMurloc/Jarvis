using Microsoft.EntityFrameworkCore;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Infrastructure.Entities;
using Sharkingbird.Jarvis.Infrastructure.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure
{
  public sealed class VacationRepository : IVacationRepository
  {
    private readonly JarvisDbContext _dbContext;
    public VacationRepository(JarvisDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<IEnumerable<Vacation>> GetVacations(CancellationToken cancellationToken)
    {
      return await _dbContext.Vacation.Select(v => new Vacation(v.Id, v.Name, v.Start, v.End)).ToArrayAsync(cancellationToken);
    }

    public async Task SaveVacations(IEnumerable<Vacation> vacations, CancellationToken cancellationToken)
    {
      var entities = vacations.Select(v => new VacationEntity
      {
        Id = v.Id,
        End = v.End,
        Name = v.Name,
        Start = v.Start
      });

      await _dbContext.AddRangeAsync(entities,cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);
    }
  }
}
