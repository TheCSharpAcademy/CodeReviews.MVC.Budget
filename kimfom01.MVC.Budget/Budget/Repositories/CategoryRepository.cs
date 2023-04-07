using Budget.Context;
using Budget.Models;

namespace Budget.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(BudgetDbContext budgetDbContext) : base(budgetDbContext)
    {
    }
}