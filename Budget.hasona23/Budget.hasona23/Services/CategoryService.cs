using Budget.hasona23.Data;
using Budget.hasona23.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.hasona23.Services;

public class CategoryService : ICategoryService
{
    private BudgetContext _context;

    public CategoryService(BudgetContext context)
    {
        _context = context;
    }
    public List<CategoryModel> GetAllCategories()
    {
        return _context.Categories.ToList();
    }

    public CategoryModel? GetCategoryById(int id)
    {
        return _context.Categories.FirstOrDefault(x=>x.Id == id)??null;
    }

    public List<TransactionModel> GetTransactionsByCategoryId(int id)
    {
        return _context.Transactions.Where(t=>t.Category.Id==id).ToList();
    }

    public void AddCategory(CategoryDto category)
    {
        try
        {
            _context.Categories.Add(new CategoryModel{Name = category.Name});
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding category {category.Name}: {ex.Message}");
            throw;
        }
    }

    public void UpdateCategory(int id,CategoryDto category)
    {
        try
        {
           _context.Update(new CategoryModel{Id = id,Name = category.Name});
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating category {category.Name}: {ex.Message}");
            throw;
        }
    }

    public void DeleteCategory(int id)
    {
        try
        {
            _context.Categories.Remove(_context.Categories.FirstOrDefault(x => x.Id == id)!);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Deleting category : {ex.Message}");
            throw;
        }   
    }

    public async Task<List<CategoryModel>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<CategoryModel?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(x=>x.Id == id);
    }

    public async Task<List<TransactionModel>> GetTransactionsByCategoryIdAsync(int id)
    {
        return await _context.Transactions.Where(t=>t.Category.Id==id).ToListAsync();
    }

    public async Task AddCategoryAsync(CategoryDto category)
    {
        try
        {
            await _context.Categories.AddAsync(new CategoryModel{Name = category.Name});
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error addingAsync category {category.Name}: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateCategoryAsync(int id,CategoryDto category)
    {
        try
        {
            _context.Update(new CategoryModel{Id = id,Name = category.Name});
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updatingAsync category {category.Name}: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteCategoryAsync(int id)
    {
        try
        {
            var categoryToDelete = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            _context.Categories.Remove(categoryToDelete!);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error DeletingAsync category : {ex.Message}");
            throw;
        }   
    }
}