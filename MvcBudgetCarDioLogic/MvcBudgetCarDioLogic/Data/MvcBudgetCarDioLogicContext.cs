using Microsoft.EntityFrameworkCore;

namespace MvcBudgetCarDioLogic.Data
{
    public class MvcBudgetCarDioLogicContext : DbContext
    {
        public MvcBudgetCarDioLogicContext(DbContextOptions<MvcBudgetCarDioLogicContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Category> Categories { get; set; }

        public DbSet<Models.Transaction> Transactions { get; set; }
    }
}
