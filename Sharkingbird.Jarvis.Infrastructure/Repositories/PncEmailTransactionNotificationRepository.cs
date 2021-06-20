using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Search;
using Microsoft.Extensions.Options;
using MimeKit;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models.Discretionary;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using Sharkingbird.Jarvis.Infrastructure.Contracts;

namespace Sharkingbird.Jarvis.Infrastructure.Repositories
{
  public sealed class PncEmailTransactionNotificationRepository : ITransactionNotificationsRepository
  {
    private readonly IEmailService _emailService;
    private readonly AccountConfiguration _accountConfiguration;
    public PncEmailTransactionNotificationRepository(IEmailService emailService,
      IOptions<AccountConfiguration> options)
    {
      _accountConfiguration = options.Value;
      _emailService = emailService;
    }

    public async Task<IEnumerable<Transaction>> GetNewDiscretionaryTransactions(CancellationToken cancellationToken)
    {
      return (await GetPncEmails(_accountConfiguration.DiscretionaryCreditCardLastFourDigits, cancellationToken))
        .Select(GetTransactionFromEmail);
    }

    public async Task<IEnumerable<Transaction>> GetNewVacationTransactions(CancellationToken cancellationToken)
    {
      return (await GetPncEmails(_accountConfiguration.VacationCreditCardLastFourDigits, cancellationToken))
        .Select(GetTransactionFromEmail);
    }
    private async Task<IEnumerable<MimeMessage>> GetPncEmails(string cardNumber, CancellationToken cancellationToken)
    {
      var search = SearchQuery.FromContains("pncalerts@visa.com")
        .And(SearchQuery.NotSeen)
        .And(SearchQuery.SubjectContains(cardNumber))
        .And(SearchQuery.SubjectContains("was recently used to make a purchase"));
      var messages = (await _emailService.SearchForMessages(search, cancellationToken)).ToArray();
      foreach (var id in messages.Select(m => m.id))
      {
        await _emailService.MarkAsRead(id, cancellationToken);
      }
      return messages.Select(m => m.content);
    }

    private static readonly Regex AmountRegex =
      new Regex(@"Transaction Amount:[^0-9]*([0-9\.]{1,})", RegexOptions.Compiled | RegexOptions.Singleline);

    private static readonly Regex DescriptionRegex =
      new Regex(@"Merchant: \<\/b\> ([^\<]*) \<br\>", RegexOptions.Compiled | RegexOptions.Singleline);
    private Transaction GetTransactionFromEmail(MimeMessage message)
    {
      var amount = AmountRegex.Match(message.HtmlBody).Groups[1].Value;
      var description = DescriptionRegex.Match(message.HtmlBody).Groups[1].Value;

      return new Transaction(Guid.NewGuid(),decimal.Parse(amount),description,message.Date);
    }

  }
}
