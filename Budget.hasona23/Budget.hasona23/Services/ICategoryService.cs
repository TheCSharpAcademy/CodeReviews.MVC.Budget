using Budget.hasona23.Models;

namespace Budget.hasona23.Services;

public interface ICategoryService
{
    List<CategoryModel> GetAllCategories();
    CategoryModel? GetCategoryById(int id);
    List<TransactionModel> GetTransactionsByCategoryId(int id);
    void AddCategory(CategoryDto category);
    void UpdateCategory(int id,CategoryDto category);
    void DeleteCategory(int id);
    Task<List<CategoryModel>> GetAllCategoriesAsync();
    Task<CategoryModel?> GetCategoryByIdAsync(int id);
    Task<List<TransactionModel>> GetTransactionsByCategoryIdAsync(int id);
    Task AddCategoryAsync(CategoryDto category);
    Task UpdateCategoryAsync(int id,CategoryDto category);
    Task DeleteCategoryAsync(int id);
}