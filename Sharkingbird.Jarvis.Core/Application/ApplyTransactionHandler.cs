﻿using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using Sharkingbird.Jarvis.Core.Mediation.Events;
using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class ApplyTransactionHandler : IRequestHandler<ApplyTransactionCommand>
  {
    private readonly IBudgetRepository _budgetRepository;
    private readonly IMediator _mediator;
    public ApplyTransactionHandler(IBudgetRepository budgetRepository,
      IMediator mediator)
    {
      _budgetRepository = budgetRepository;
      _mediator = mediator;
    }
    public async Task<Unit> Handle(ApplyTransactionCommand request, CancellationToken cancellationToken)
    {
      var budget = await _budgetRepository.GetBudget(request.BudgetName, cancellationToken);
      budget.ApplyTransaction(new Transaction(Guid.NewGuid(), request.Amount, request.Description, DateTimeOffset.Now, null);

      await _budgetRepository.SaveBudget(budget, cancellationToken);
      await _mediator.Publish(new TransactionsAppliedEvent(budget), cancellationToken);
      return Unit.Value;
    }
  }
}
