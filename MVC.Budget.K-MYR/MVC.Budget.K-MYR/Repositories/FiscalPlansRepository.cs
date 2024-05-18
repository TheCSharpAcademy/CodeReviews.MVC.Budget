using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public class FiscalPlansRepository : GenericRepository<FiscalPlan>, IFiscalPlansRepository
{
    public FiscalPlansRepository(DatabaseContext context) : base(context)
    { }
}
