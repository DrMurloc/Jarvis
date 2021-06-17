using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Models.Discretionary;
using Sharkingbird.Jarvis.Core.Models.Events;
using Sharkingbird.Jarvis.Core.Models.Vacation;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Infrastructure.Services
{
  public sealed class BudgetService : IBudgetService,
    INotificationHandler<DiscretionaryBudgetBalanceModifiedEvent>,
    INotificationHandler<VacationBudgetBalanceModifiedEvent>
  {
    private readonly IVacationRepository _vacationRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMediator _mediator;
    public BudgetService(IVacationRepository vacationRepository,
      IBudgetRepository budgetRepository,
      ITransactionRepository transactionRepository,
      IMediator mediator)
    {
      _vacationRepository = vacationRepository;
      _budgetRepository = budgetRepository;
      _transactionRepository = transactionRepository;
      _mediator = mediator;
    }
    public async Task<DiscretionaryBudget> GetDiscretionaryBudget(CancellationToken cancellationToken)
    {
      var budget = await _budgetRepository.GetBudgetByName("Discretionary", cancellationToken);
      var transactions = await _transactionRepository.GetDiscretionaryTransactions(cancellationToken);
      return new DiscretionaryBudget(_mediator, budget.Allowance, budget.Balance, transactions);
    }

    public async Task<VacationBudget> GetVacationBudget(CancellationToken cancellationToken)
    {
      var budget = await _budgetRepository.GetBudgetByName("Vacation", cancellationToken);
      var vacations = await _vacationRepository.GetUpcomingCalanderEvents(cancellationToken);
      return new VacationBudget(_mediator, budget.Allowance, budget.Balance, vacations);
    }

    public async Task Handle(DiscretionaryBudgetBalanceModifiedEvent notification, CancellationToken cancellationToken)
    {
      await _budgetRepository.UpdateBudgetBalance("Discretionary", notification.NewBalance, cancellationToken);
    }

    public async Task Handle(VacationBudgetBalanceModifiedEvent notification, CancellationToken cancellationToken)
    {
      await _budgetRepository.UpdateBudgetBalance("Vacation", notification.NewBalance, cancellationToken);
    }
  }
}
