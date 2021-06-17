using System;

namespace Sharkingbird.Jarvis.Core.Models
{
  public sealed class Notification
  {
    public Notification(string messageParam)
    {
      Message = messageParam ?? throw new ArgumentNullException(nameof(messageParam));
    }
    public string Message { get; }
  }
}
