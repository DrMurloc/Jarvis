using Microsoft.Extensions.Logging;
using MimeKit;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Infrastructure.Abstractions;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
using System;
using System.Text.RegularExpressions;

namespace Sharkingbird.Jarvis.Infrastructure.RecurringPayments
{
  public sealed class WsscRecurringPaymentRepository : EmailRecurringPaymentRepository
  {
    public WsscRecurringPaymentRepository(IEmailService emailService,
      ILogger<WsscRecurringPaymentRepository> loggerParam) : base(emailService, loggerParam)
    {
    }

    protected override string FromContains => "donotreply@wssc.ebillservice.net";

    protected override string SubjectContains => "Automatic payment has been scheduled";

    private static Regex _regex = new Regex(@"Payment Date[^0-9]*([0-9\/]*).*Payment Amount[^0-9]*([0-9\.]*)", RegexOptions.Compiled | RegexOptions.Singleline);
    protected override RecurringPayment GetPaymentFromMessage(MimeMessage message)
    {
      var match = _regex.Match(message.HtmlBody);
      var amount = match.Groups[2].Value;
      var date = match.Groups[1].Value;
      return new RecurringPayment("Wssc (Water)", DateTimeOffset.Parse(date), double.Parse(amount));
    }
  }
}
