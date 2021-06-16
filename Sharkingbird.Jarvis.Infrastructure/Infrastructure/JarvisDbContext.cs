using Microsoft.EntityFrameworkCore;
using Sharkingbird.Jarvis.Infrastructure.Entities;

namespace Sharkingbird.Jarvis.Infrastructure.Infrastructure
{
  public sealed class JarvisDbContext : DbContext
  {
    public JarvisDbContext(DbContextOptions<JarvisDbContext> optionsParam) : base(optionsParam) { }
    public DbSet<BudgetEntity> Budget { get; set; }
    public DbSet<TransactionEntity> Transaction { get; set; }
    public DbSet<RecurringTransactionEntity> RecurringTransaction { get; set; }
    public DbSet<VacationEntity> Vacation { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("jarvis");
    }
  }
}
