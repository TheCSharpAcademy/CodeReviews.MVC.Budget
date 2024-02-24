using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;
using System.Linq.Expressions;


namespace MVC.Budget.K_MYR.Repositories;

public sealed class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
{
    public CategoriesRepository(DatabaseContext context) : base(context)
    { }

    public Task<Category?> GetCategoryAsync(int id)
    {
        return _dbSet
                .Include(c => c.Transactions.OrderByDescending(t => t.DateTime))
                .SingleOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetCategoryWithFilteredTransactionsAsync(Expression<Func<Category, bool>>? filter = null, Func<IQueryable<Category>,
                                                IOrderedQueryable<Category>>? orderBy = null,
                                                Expression<Func<Category,
                                                IOrderedEnumerable<Transaction>>>? filterTransactions = null)
    {
        IQueryable<Category> query = _dbSet;

        if (filter is not null)
            query = query.Where(filter);

        if (filterTransactions is not null)
        {
            query = query.Include(filterTransactions);
        }

        if (orderBy is not null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }
}
