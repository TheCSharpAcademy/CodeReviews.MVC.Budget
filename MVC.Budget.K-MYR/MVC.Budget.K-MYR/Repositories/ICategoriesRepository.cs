using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public interface ICategoriesRepository : IGenericRepository<Category>
{
    Task<Category?> GetCategoryAsync(int id);
}