using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Sharkingbird.Jarvis.Core;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure.Abstractions
{
  public abstract class EmailRecurringPaymentRepository : IRecurringPaymentRepository
  {
    private readonly ILogger _logger;
    private readonly EmailConfiguration _configuration;
    protected EmailRecurringPaymentRepository(IOptions<EmailConfiguration> optionsParam,
      ILogger loggerParam)
    {
      _logger = loggerParam;
      _configuration = optionsParam.Value;
    }
    protected abstract RecurringPayment GetPaymentFromMessage(MimeMessage message);
    protected abstract string FromContains { get; }
    protected abstract string SubjectContains { get; }
    public async Task<IEnumerable<RecurringPayment>> GetNewRecurringPayments(CancellationToken cancellationToken)
    {

      var client = new ImapClient();

      await client.ConnectAsync("imap.gmail.com", 993, true,cancellationToken);
      await client.AuthenticateAsync(_configuration.UserName, _configuration.Password,cancellationToken);

      var inbox = client.Inbox;
      await inbox.OpenAsync(MailKit.FolderAccess.ReadWrite,cancellationToken);
      var search = SearchQuery.FromContains(FromContains)
        .And(SearchQuery.NotSeen)
        .And(SearchQuery.SubjectContains(SubjectContains));

      var existingMessages = await inbox.SearchAsync(search, cancellationToken);
      
      var payments = new List<RecurringPayment>();
      foreach (var id in existingMessages)
      {
        var message = await inbox.GetMessageAsync(id, cancellationToken);
        try
        {
          payments.Add(GetPaymentFromMessage(message));
          await inbox.AddFlagsAsync(id, MailKit.MessageFlags.Seen,false,cancellationToken);

        } catch(Exception e)
        {
          _logger.LogError(e, $"Error parsing Pepco email");
        }
      }
      return payments;
    }

  }
}
