using Budget.Doc415.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.Doc415.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategories();
    Task DeleteCategory(int id);
    Task InsertCategory(Category category);
    Task UpdateCategory(Category category);
    Task<SelectList> GetCategories();
    Task<int> GetCategoryByName(string name);
}
