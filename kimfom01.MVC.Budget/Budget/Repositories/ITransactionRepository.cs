using System.Linq.Expressions;
using Budget.Models;

namespace Budget.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    public Task<IEnumerable<Transaction>?> GetTransactionsWithCategories(Expression<Func<Transaction, bool>> predicate);
}