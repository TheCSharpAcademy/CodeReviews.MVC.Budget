using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public interface ITransactionsRepository : IGenericRepository<Transaction>
{
    Task<List<TransactionDTO>> GetAsync(TransactionsSearch searchModel, Func<IQueryable<Transaction>,
                                                 IOrderedQueryable<Transaction>> orderBy, Func<IQueryable<Transaction>,
                                                 IQueryable<Transaction>>? filter = null);
    Task<List<Transaction>> GetUnevaluatedTransactions(int categoryid, int? lastId = null, DateTime? lastDate = null, int pageSize = 10);
}