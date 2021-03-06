using Microsoft.Extensions.Logging;
using MimeKit;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Infrastructure.Abstractions;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
using System;
using System.Text.RegularExpressions;

namespace Sharkingbird.Jarvis.Infrastructure.RecurringPayments
{
  public sealed class VivintRecurringPaymentRepository : EmailRecurringPaymentRepository
  {
    public VivintRecurringPaymentRepository(IEmailService emailService,
      ILogger<VivintRecurringPaymentRepository> loggerParam) : base(emailService, loggerParam)
    {
    }

    protected override string FromContains => "VivintSolar_No_Reply@billerpayments.com";

    protected override string SubjectContains => "Your New Vivint Solar Billing Statement is Available";

    private static Regex _regex = new Regex(@"Payment Due Date:\s*([0-9\/]*)[^\$]*\$([0-9\.]*)", RegexOptions.Compiled | RegexOptions.Singleline);

    protected override RecurringPayment GetPaymentFromMessage(MimeMessage message)
    {
      var match = _regex.Match(message.HtmlBody);
      var amount = match.Groups[2].Value;
      var date = match.Groups[1].Value;
      return new RecurringPayment("Vivint (Solar)", DateTimeOffset.Parse(date), double.Parse(amount));
    }
  }
}
