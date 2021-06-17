using System;
using System.ComponentModel.DataAnnotations;

namespace Sharkingbird.Jarvis.Infrastructure.Entities
{
  public sealed class BudgetEntity
  {
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Balance { get; set; }
    [Required]
    public decimal Allowance { get; set; }
  }
}
