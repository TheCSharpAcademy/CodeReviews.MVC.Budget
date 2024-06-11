using Budget.Doc415.Data;
using Budget.Doc415.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Budget.Doc415.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbContextFactory<BudgetDb> _fcontext;

        public CategoryRepository(IDbContextFactory<BudgetDb> fcontext)
        {
            _fcontext = fcontext;
        }
        public async Task<List<Category>> GetAllCategories()
        {
            try
            {
                using var _context = _fcontext.CreateDbContext();
                var categories = await _context.Categories.ToListAsync();
                return categories;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Database exception: {ex}");
                return await Task.FromResult(new List<Category>());
            }
        }

        public async Task<SelectList> GetCategories()
        {
            using var _context = _fcontext.CreateDbContext();
            var categoryList = new SelectList(await _context.Categories.Select(x => x.Name).Distinct().ToListAsync());
            return categoryList;
        }

        public async Task<int> GetCategoryByName(string name)
        {
            using var _context = _fcontext.CreateDbContext();
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == name);
            if (category != null)
                return category.Id;
            else
                return -1;


        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                using var _context = _fcontext.CreateDbContext();
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Database exception: {ex}");
            }
        }

        public async Task InsertCategory(Category category)
        {
            try
            {
                using var _context = _fcontext.CreateDbContext();
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Database exception: {ex}");
            }
        }

        public async Task UpdateCategory(Category category)
        {
            try
            {
                using var _context = _fcontext.CreateDbContext();
                var toUpdate = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
                toUpdate.Name = category.Name;
                _context.Update(toUpdate);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Database exception: {ex}");
            }
        }
    }
}
