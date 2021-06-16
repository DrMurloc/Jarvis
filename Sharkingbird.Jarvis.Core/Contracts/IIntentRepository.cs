using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Contracts
{
  public interface IIntentRepository
  {
    Task<IRequest> GetRequestFromIntent(string request, CancellationToken cancellationToken);
  }
}
