using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sharkingbird.Jarvis.Infrastructure.Entities
{
  public sealed class TransactionEntity
  {
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid BudgetId { get; set; }
    public Guid? RecurringTransactionId { get; set; }
    [Required]
    public double Amount { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTimeOffset AppliedOn { get; set; }
  }
}
