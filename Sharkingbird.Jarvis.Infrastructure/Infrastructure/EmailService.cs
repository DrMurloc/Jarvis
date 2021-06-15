using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Options;
using MimeKit;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure.Infrastructure
{
  public sealed class EmailService : IEmailService
  {
    private readonly EmailConfiguration _configuration;
    public EmailService(IOptions<EmailConfiguration> options)
    {
      _configuration = options.Value;
    }
    private IMailFolder _inbox;
    private async Task<IMailFolder> GetInbox(CancellationToken cancellationToken)
    {

      if (_inbox != null)
      {
        return _inbox;
      }
      var client = new ImapClient();

      await client.ConnectAsync("imap.gmail.com", 993, true, cancellationToken);
      await client.AuthenticateAsync(_configuration.UserName, _configuration.Password, cancellationToken);

      _inbox = client.Inbox;
      await _inbox.OpenAsync(FolderAccess.ReadWrite, cancellationToken);
      return _inbox;
    }
    public async Task<IEnumerable<(UniqueId, MimeMessage)>> SearchForMessages(SearchQuery query, CancellationToken cancellationToken)
    {
      var inbox = await GetInbox(cancellationToken);
      var existingMessages = await inbox.SearchAsync(query, cancellationToken);
      var result = new List<(UniqueId, MimeMessage)>();
      foreach (var id in existingMessages)
      {
        result.Add((id, await _inbox.GetMessageAsync(id, cancellationToken)));
      }
      return result;
    }

    public async Task MarkAsRead(UniqueId messageId, CancellationToken cancellationToken)
    {
      await _inbox.AddFlagsAsync(messageId, MessageFlags.Seen, false, cancellationToken);
    }
  }
}
