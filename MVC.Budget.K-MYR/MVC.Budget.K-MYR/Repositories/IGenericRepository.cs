using MVC.Budget.K_MYR.Models;
using System.Linq.Expressions;

namespace MVC.Budget.K_MYR.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "");
    TEntity? GetByID(int id);
    ValueTask<TEntity?> GetByIDAsync(int id);
    void Insert(TEntity category);
    void Update(TEntity category);
    Task DeleteAsync(int id);
}