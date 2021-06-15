using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Infrastructure.Abstractions;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sharkingbird.Jarvis.Infrastructure.RecurringPayments
{
  public sealed class WashingtonianGasRecurringPaymentRepository : EmailRecurringPaymentRepository
  {
    public WashingtonianGasRecurringPaymentRepository(IEmailService emailService,
      ILogger<WashingtonianGasRecurringPaymentRepository> loggerParam) : base(emailService, loggerParam)
    {
    }

    protected override string FromContains => "customersupport@washgas.com";

    protected override string SubjectContains => "Your new gas bill from Washington Gas";

    private static Regex _amountRegex = new Regex(@"\$([0-9\.]*)", RegexOptions.Compiled);
    private static Regex _dateRegex = new Regex(@"Scheduled for automatic payment on ([A-Za-z0-9 ]*\,[ 0-9]*)", RegexOptions.Compiled | RegexOptions.Singleline);
    protected override RecurringPayment GetPaymentFromMessage(MimeMessage message)
    {
      var amountMatch = _amountRegex.Match(message.HtmlBody).Groups[1].Value;
      var date = _dateRegex.Match(message.HtmlBody).Groups[1].Value;
      return new RecurringPayment("Washingtonian Gas", DateTimeOffset.Parse(date), double.Parse(amountMatch));
    }
  }
}
