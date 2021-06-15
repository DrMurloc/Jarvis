using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sharkingbird.Jarvis.Infrastructure.Entities
{
  public sealed class RecurringTransactionEntity
  {
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public Guid BudgetId { get; set; }
    [Required]
    public double Amount { get; set; }
    [Required]
    public string Rate { get; set; }
  }
}
