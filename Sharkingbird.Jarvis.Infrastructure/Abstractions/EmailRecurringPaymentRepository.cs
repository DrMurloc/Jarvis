using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Sharkingbird.Jarvis.Core;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
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
    private readonly IEmailService _emailService;
    protected EmailRecurringPaymentRepository(IEmailService emailService,
      ILogger loggerParam)
    {
      _logger = loggerParam;
      _emailService = emailService;
    }
    protected abstract RecurringPayment GetPaymentFromMessage(MimeMessage message);
    protected abstract string FromContains { get; }
    protected abstract string SubjectContains { get; }
    public async Task<IEnumerable<RecurringPayment>> GetNewRecurringPayments(CancellationToken cancellationToken)
    {
      var search = SearchQuery.FromContains(FromContains)
        .And(SearchQuery.NotSeen)
        .And(SearchQuery.SubjectContains(SubjectContains));

      var existingMessages = await _emailService.SearchForMessages(search, cancellationToken);
      
      var payments = new List<RecurringPayment>();
      foreach (var message in existingMessages)
      {
        try
        {
          payments.Add(GetPaymentFromMessage(message.content));
          await _emailService.MarkAsRead(message.id,cancellationToken);

        } catch(Exception e)
        {
          _logger.LogError(e, $"Error parsing Pepco email");
        }
      }
      return payments;
    }

  }
}
