using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;
using System.Linq.Expressions;

namespace MVC.Budget.K_MYR.Repositories;

public class CategoriesRepository : ICategoriesRepository
{
    private readonly DatabaseContext _context;

    public CategoriesRepository(DatabaseContext context)
    {
        _context = context;
    }
    public Task<List<Category>> GetCategoriesAsync(Expression<Func<Category, bool>>? filter = null, Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null )
    {
        IQueryable<Category> query = _context.Categories;

        if (filter != null)
            query = query.Where(filter).Include(c => c.Transactions);

        if(orderBy != null)
            return orderBy(query).ToListAsync();

        return query.ToListAsync();
    }

    public Category? GetCategory(int id)
    {
        return _context.Categories.Find(id);
    }

    public ValueTask<Category?> GetCategoryAsync(int id)
    {
        return _context.Categories.FindAsync(id);
    }

    public async Task AddCategoryAsnyc(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsnyc(Category category)
    {
        _context.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsnyc(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category is not null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}

public interface ICategoriesRepository
{
    Task<List<Category>> GetCategoriesAsync(Expression<Func<Category, bool>>? filter = null, Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null);
    Category? GetCategory(int id);
    ValueTask<Category?> GetCategoryAsync(int id);
    Task AddCategoryAsnyc(Category category);
    Task UpdateCategoryAsnyc(Category category);
    Task DeleteCategoryAsnyc(int id);

}