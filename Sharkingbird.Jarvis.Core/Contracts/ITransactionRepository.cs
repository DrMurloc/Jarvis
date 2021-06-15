using Sharkingbird.Jarvis.Core.Models;
using Sharkingbird.Jarvis.Core.Models.Enums;
using Sharkingbird.Jarvis.Core.Models.ValueTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Contracts
{
  public interface ITransactionRepository
  {
    Task<IEnumerable<RecurringTransaction>> GetRecurringTransactions(CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetTransactionsByBudget(BudgetNameValueType budgetName, CancellationToken cancellationToken);
    Task SaveTransactions(BudgetNameValueType budgetName, IEnumerable<Transaction> transactions, CancellationToken cancellationToken);
  }
}
