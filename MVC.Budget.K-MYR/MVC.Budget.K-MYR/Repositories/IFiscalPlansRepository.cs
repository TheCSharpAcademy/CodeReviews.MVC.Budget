using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public interface IFiscalPlansRepository : IGenericRepository<FiscalPlan>
{
    Task<FiscalPlanByYear?> GetDataByYear(int id, int year);
}
