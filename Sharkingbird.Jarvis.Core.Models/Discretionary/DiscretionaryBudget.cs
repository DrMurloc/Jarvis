using MediatR;
using Sharkingbird.Jarvis.Core.Models.Events;
using System.Collections.Generic;
using System.Linq;

namespace Sharkingbird.Jarvis.Core.Models.Discretionary
{
  public sealed class DiscretionaryBudget
  {
    private readonly IMediator _mediator;
    public DiscretionaryBudget(IMediator mediator, decimal weeklyAllowance, decimal balance, IEnumerable<Transaction> transactions)
    {
      _mediator = mediator;
      _transactions = transactions.ToList();
      Balance = balance;
      WeeklyAllowance = weeklyAllowance;
    }
    public decimal WeeklyAllowance { get; }
    private readonly IList<Transaction> _transactions;
    public IEnumerable<Transaction> Transactions => _transactions;
    public decimal Balance { get; private set; }

    public void ApplyTransactions(IEnumerable<Transaction> transactions)
    {
      foreach (var transaction in transactions)
      {
        _transactions.Add(transaction);
        Balance -= transaction.Amount;
      }

      _mediator.Publish(new TransactionsAppliedEvent(transactions)).Wait();
      _mediator.Publish(new DiscretionaryBudgetBalanceModifiedEvent(Balance)).Wait();
    }

    public void ApplyTransaction(Transaction transaction) => ApplyTransactions(new[] {transaction});
    public void ApplyAllowance()
    {
      Balance += WeeklyAllowance;
      _mediator.Publish(new DiscretionaryBudgetBalanceModifiedEvent(Balance)).Wait();
    }
  }
}
