using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using System.Linq.Expressions;

namespace MVC.Budget.K_MYR.Repositories;
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly DatabaseContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DatabaseContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>,
                                                 IOrderedQueryable<TEntity>>? orderBy = null,
                                                 string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter is not null)
            query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split
            (',', StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy is not null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public virtual TEntity? GetByID(int id)
    {
        return _dbSet.Find(id);
    }

    public virtual ValueTask<TEntity?> GetByIDAsync(int id)
    {
        return _dbSet.FindAsync(id);
    }

    public virtual void Insert(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
    }
}
