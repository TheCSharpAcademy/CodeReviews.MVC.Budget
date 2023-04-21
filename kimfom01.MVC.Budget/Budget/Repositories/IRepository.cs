using System.Linq.Expressions;

namespace Budget.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task AddEntity(TEntity entity);
    Task RemoveEntity(int id);
    Task Update(int id, TEntity entity);
    Task SaveChanges();
    Task<IEnumerable<TEntity>?> GetEntities(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetEntity(int? id);
}