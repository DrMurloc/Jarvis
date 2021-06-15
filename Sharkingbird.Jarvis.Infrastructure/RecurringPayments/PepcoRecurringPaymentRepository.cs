using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Infrastructure.Abstractions;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
using System;
using System.Text.RegularExpressions;

namespace Sharkingbird.Jarvis.Infrastructure.RecurringPayments
{
  public sealed class PepcoRecurringPaymentRepository : EmailRecurringPaymentRepository
  {
    public PepcoRecurringPaymentRepository(IEmailService emailService,
      ILogger<PepcoRecurringPaymentRepository> loggerParam) : base(emailService,loggerParam)
    {
    }

    protected override string FromContains => "no-reply@pepco.com";

    protected override string SubjectContains => "Your Payment Has Been Scheduled";

    private static Regex _pepcoRegex = new Regex(@"Your scheduled payment of [^\$]*\$([0-9\.]*)[^0-9]*([0-9\/]*)", RegexOptions.Compiled | RegexOptions.Singleline);
    protected override RecurringPayment GetPaymentFromMessage(MimeMessage message)
    {
      var match = _pepcoRegex.Match(message.HtmlBody);
      var amount = match.Groups[1].Value;
      var date = match.Groups[2].Value;
      return new RecurringPayment("Pepco (Electricity)", DateTimeOffset.Parse(date), double.Parse(amount));
    }
  }
}
