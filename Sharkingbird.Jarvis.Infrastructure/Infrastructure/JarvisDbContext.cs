using System;
using Microsoft.EntityFrameworkCore;
using Sharkingbird.Jarvis.Infrastructure.Entities;

namespace Sharkingbird.Jarvis.Infrastructure.Infrastructure
{
  public sealed class JarvisDbContext : DbContext
  {
    public JarvisDbContext(DbContextOptions<JarvisDbContext> optionsParam) : base(optionsParam) { }
    public DbSet<BudgetEntity> Budget { get; set; }
    public DbSet<TransactionEntity> Transaction { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<BudgetEntity>().ToContainer("Budget")
        .HasPartitionKey(m=>m.Name)
        .HasNoDiscriminator()
        .Property(m=>m.Id)
        .HasConversion(g=>g.ToString(),s=>Guid.Parse(s));

      modelBuilder.Entity<TransactionEntity>().ToContainer("Transaction")
        .HasPartitionKey(m => m.Description)
        .HasNoDiscriminator()
        .Property(m => m.Id)
        .HasConversion(g => g.ToString(), s => Guid.Parse(s)); ;
      
    }
  }
}
