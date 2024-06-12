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

    public async Task<FiscalPlanDTO> GetDataByMonth(int id, DateTime Month)
    {
        var categorieData = await _unitOfWork.CategoriesRepository.GetDataByMonth(id, Month);       

        return new FiscalPlanDTO
        {
            Id = id,
            IncomeCategories = categorieData?.IncomeCategories?.OrderBy(c => c.Name)?.ToList() ?? [],
            ExpenseCategories = categorieData?.ExpenseCategories?.OrderBy(c => c.Name)?.ToList() ?? [],
            ExpensesTotal = categorieData?.ExpenseCategories.Sum(c => c.Total) ?? 0,
            ExpensesBudget = categorieData?.ExpenseCategories.Sum(c => c.BudgetLimit?.Budget ?? c.Budget) ?? 0,
            ExpensesHappyTotal = categorieData?.ExpenseCategories.Sum(c => c.HappyTotal) ?? 0,
            ExpensesNecessaryTotal = categorieData?.ExpenseCategories.Sum(c => c.NecessaryTotal) ?? 0,
            IncomeTotal = categorieData?.IncomeCategories.Sum(c => c.Total) ?? 0,
            IncomeBudget = categorieData?.IncomeCategories.Sum(c => c.BudgetLimit?.Budget ?? c.Budget) ?? 0,
            Overspending = categorieData?.ExpenseCategories.Sum(c => Math.Max(0, c.Total - (c.BudgetLimit?.Budget ?? c.Budget))) ?? 0
        };    
    }

    public async Task<YearlyStatisticsDto> GetDataByYear(int fiscalPlanId, int year)
    {
        var fiscalPlanDTO = await _unitOfWork.CategoriesRepository.GetDataByYear(fiscalPlanId, year);

        var grouped = fiscalPlanDTO?.CategoryStatistics.SelectMany(a => a.Statistics).GroupBy(s => s.Month);

        var months = Enumerable.Range(1, 12);

        var stats = new YearlyStatisticsDto
        {
            TotalPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.TotalSpent) ?? 0),
            HappyPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.HappyTransactions) ?? 0),
            HappyEvaluatedPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.HappyEvaluatedTransactions) ?? 0),
            UnhappyPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnhappyTransactions) ?? 0),
            UnhappyEvaluatedPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnhappyEvaluatedTransactions) ?? 0),
            NecessaryPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.NecessaryTransactions) ?? 0),
            NecessaryEvaluatedPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.NecessaryEvaluatedTransactions) ?? 0),
            UnnecessaryPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnnecessaryTransactions) ?? 0),
            UnnecessaryEvaluatedPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnnecessaryEvaluatedTransactions) ?? 0),
            UnevaluatedPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnevaluatedTransactions) ?? 0),
            MonthlyOverspendingPerCategory = fiscalPlanDTO?.CategoryStatistics?.OrderBy(c => c.Category).Select(c => new MonthlyOverspendingPerCategory
            {
                Category = c.Category,
                OverspendingPerMonth = months.Select(month =>
                    Math.Max(0, (c.Statistics.FirstOrDefault(s => s.Month == month)?.TotalSpent ?? 0)
                    - (c.BudgetLimits.LastOrDefault(bl => bl.Month.Month <= month)?.Budget ?? c.Budget)))
            }) ?? []
        };    

        stats.TotalSpent = stats.TotalPerMonth.Sum();
        stats.OverspendingTotal = stats.MonthlyOverspendingPerCategory.SelectMany(s => s.OverspendingPerMonth).Sum();
        stats.HappyEvaluatedTotal = stats.HappyEvaluatedPerMonth.Sum();
        stats.UnhappyEvaluatedTotal = stats.TotalSpent - stats.HappyEvaluatedTotal;
        stats.NecessaryEvaluatedTotal = stats.NecessaryEvaluatedPerMonth.Sum();
        stats.UnnecessaryEvaluatedTotal = stats.TotalSpent - stats.NecessaryEvaluatedTotal;

        return stats;
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
}
