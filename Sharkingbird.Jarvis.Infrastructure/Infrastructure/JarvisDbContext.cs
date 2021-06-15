using Microsoft.EntityFrameworkCore;
using Sharkingbird.Jarvis.Infrastructure.Entities;

namespace Sharkingbird.Jarvis.Infrastructure.Infrastructure
{
  public sealed class JarvisDbContext : DbContext
  {
    public DbSet<BudgetEntity> Budget { get; set; }
  }
}
