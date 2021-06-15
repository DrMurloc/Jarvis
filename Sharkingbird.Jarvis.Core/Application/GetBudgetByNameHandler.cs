using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Queries;
using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class GetBudgetByNameHandler : IRequestHandler<GetBudgetByNameQuery,Budget>
  {
    private readonly IBudgetRepository _repository;
    public GetBudgetByNameHandler(IBudgetRepository repository)
    {
      _repository = repository;
    }
    public async Task<Budget> Handle(GetBudgetByNameQuery request, CancellationToken cancellationToken)
    {
      return await _repository.GetBudget(request.BudgetName, cancellationToken);
    }
  }
}
