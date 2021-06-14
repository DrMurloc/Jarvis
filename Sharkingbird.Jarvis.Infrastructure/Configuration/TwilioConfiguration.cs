using System;
using System.Collections.Generic;
using System.Text;

namespace Sharkingbird.Jarvis.Infrastructure.Configuration
{
  public sealed class TwilioConfiguration
  {
    public string AccountId { get; set; }
    public string AuthToken { get; set; }
    public string FromPhoneNumber { get; set; }
    public string[] ToPhoneNumbers { get; set; }
  }
}
