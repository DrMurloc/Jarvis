using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sharkingbird.Jarvis.Infrastructure.Entities
{
  public sealed class VacationEntity
  {
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTimeOffset Start { get; set; }
    [Required]
    public DateTimeOffset End { get; set; }
  }
}
