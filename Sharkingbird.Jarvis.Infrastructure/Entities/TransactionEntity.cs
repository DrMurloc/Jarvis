using System;
using System.ComponentModel.DataAnnotations;

namespace Sharkingbird.Jarvis.Infrastructure.Entities
{
  public sealed class TransactionEntity
  {
    [Key]
    public Guid Id { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTimeOffset AppliedOn { get; set; }
  }
}
