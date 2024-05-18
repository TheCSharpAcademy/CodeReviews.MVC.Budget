using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Services;

public class FiscalPlansService : IFiscalPlansService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FiscalPlansService> _logger;

    public FiscalPlansService(IUnitOfWork unitOfWork, ILogger<FiscalPlansService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Task<List<FiscalPlan>> GetFiscalPlans()
    {
        return _unitOfWork.FiscalPlansRepository.GetAsync();
    }

    public ValueTask<FiscalPlan?> GetByIDAsync(int id)
    {
        return _unitOfWork.FiscalPlansRepository.GetByIDAsync(id);
    }

    public FiscalPlan? GetByID(int id)
    {
        return _unitOfWork.FiscalPlansRepository.GetByID(id);
    }

    public async Task<FiscalPlan> AddFiscalPlan(FiscalPlanPost fiscaPlanPost)
    {
        var fiscalPlan = new FiscalPlan()
        {
            Name = fiscaPlanPost.Name,
        };

        _unitOfWork.FiscalPlansRepository.Insert(fiscalPlan);

        await _unitOfWork.Save();

        return fiscalPlan;
    }

    public async Task UpdateFiscalPlan(FiscalPlan fiscalPlan, FiscalPlanPut fiscalPlanPut)
    {
        fiscalPlan.Name = fiscalPlanPut.Name;

        await _unitOfWork.Save();
    }

    public async Task DeleteFiscalPlan(FiscalPlan fiscalPlan)
    {
        _unitOfWork.FiscalPlansRepository.Delete(fiscalPlan);
        await _unitOfWork.Save();
    }

    public async Task<bool> FiscalPlanExists(int id) => await _unitOfWork.FiscalPlansRepository.GetByIDAsync(id) is not null;

}
