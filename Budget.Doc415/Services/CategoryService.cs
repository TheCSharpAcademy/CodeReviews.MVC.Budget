using Budget.Doc415.Models;
using Budget.Doc415.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.Doc415.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        return await _categoryRepository.GetAllCategories();
    }

    public async Task<int> GetCategoryByName(string name)
    {
        return await _categoryRepository.GetCategoryByName(name);
    }

    public async Task<SelectList> GetCategoriesForPullDown()
    {
        return await _categoryRepository.GetCategories();
    }

    public async Task DeleteCategory(int id)
    {
        await _categoryRepository.DeleteCategory(id);
    }

    public async Task InsertCategory(Category category)
    {
        await _categoryRepository.InsertCategory(category);
    }

    public async Task UpdateCategory(Category category)
    {
        await _categoryRepository.UpdateCategory(category);
    }
}

