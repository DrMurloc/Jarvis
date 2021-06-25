using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Core.Models.Helpers
{
  public static class DateTimeOffsetExtensions
  {
    public static DateTimeOffset StartOfDay(this DateTimeOffset dateTime)
    {
      return new DateTimeOffset(
        dateTime.Year,
        dateTime.Month,
        dateTime.Day,
        0, 0, 0, dateTime.Offset);
    }
    public static DateTimeOffset EndOfDay(this DateTimeOffset dateTime)
    {
      return new DateTimeOffset(
        dateTime.Year,
        dateTime.Month,
        dateTime.Day,
        23, 59, 59, dateTime.Offset);
    }
  }
}
