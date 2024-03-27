using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public class CategoryBudgetsRepository : GenericRepository<CategoryBudget>, ICategoryBudgetsRepository
{
    public CategoryBudgetsRepository(DatabaseContext context) : base(context)
    { }
}
