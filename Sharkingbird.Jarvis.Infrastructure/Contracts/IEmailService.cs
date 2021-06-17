using MailKit;
using MailKit.Search;
using MimeKit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure.Contracts
{
  public interface IEmailService
  {
    Task<IEnumerable<(UniqueId id,MimeMessage content)>> SearchForMessages(SearchQuery query, CancellationToken cancellationToken);
    Task MarkAsRead(UniqueId messageId, CancellationToken cancellationToken);
  }
}
