using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Models.Enums
{
  public enum RecurringRateEnum
  {
    Weekly
  }
  public static class RecurringRateEnumExtensionMethods
  {
    public static TimeSpan GetTimeSpan(this RecurringRateEnum rate) => rate switch
    {
      RecurringRateEnum.Weekly => TimeSpan.FromDays(7),
      _ => TimeSpan.FromDays(7)
    };
  }
}
