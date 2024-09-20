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

    public Task<List<FiscalPlanDTO>> GetFiscalPlanDTOs(DateTime? month = null)
    {        
        var date = month ?? DateTime.UtcNow;
        return _unitOfWork.FiscalPlansRepository.GetAllWithMonthlyData(date);
    }

    public ValueTask<FiscalPlan?> GetByIDAsync(int id)
    {
        return _unitOfWork.FiscalPlansRepository.GetByIDAsync(id);
    }

    public FiscalPlan? GetByID(int id)
    {
        return _unitOfWork.FiscalPlansRepository.GetByID(id);
    }

    public async Task<FiscalPlanMonthDTO> GetDataByMonth(FiscalPlan fiscalPlan, DateTime Month)
    {
        var categorieData = await _unitOfWork.CategoriesRepository.GetDataByMonth(fiscalPlan.Id, Month);
        var incomeCategories = new List<CategoryDTO>();
        var expenseCategories = new List<CategoryDTO>();

        decimal expensesTotal = 0, expensesBudget = 0, expensesHappyTotal = 0, expensesNecessaryTotal = 0, incomeTotal = 0, incomeBudget = 0, overspending = 0;

        for(int i = 0; i < categorieData.Count; i++)
        {
            var category  = categorieData[i];
            if (category.CategoryType == 1)
            {
                incomeCategories.Add(category);
                incomeTotal += category.Total;
                incomeBudget += category.BudgetLimit?.Budget ?? category.Budget;
            }
            else if (category.CategoryType == 2)
            {
                expenseCategories.Add(category);
                expensesTotal += category.Total;
                expensesBudget += category.BudgetLimit?.Budget ?? category.Budget;
                expensesHappyTotal += category.HappyTotal;
                expensesNecessaryTotal += category.NecessaryTotal;
                overspending += Math.Max(0, category.Total - (category.BudgetLimit?.Budget ?? category.Budget));
            }
        }

        return new FiscalPlanMonthDTO
        {
            Month = Month,
            Name = fiscalPlan.Name,
            Id = fiscalPlan.Id,
            IncomeCategories = [.. incomeCategories.OrderBy(c => c.Name)],
            ExpenseCategories = [.. expenseCategories.OrderBy(c => c.Name)],
            ExpensesTotal = expensesTotal,
            ExpensesBudget = expensesBudget,
            ExpensesHappyTotal = expensesHappyTotal,
            ExpensesNecessaryTotal = expensesNecessaryTotal,
            IncomeTotal = incomeTotal,
            IncomeBudget = incomeBudget,
            Overspending = overspending
        };
    }

    public async Task<FiscalPlanYearDTO> GetDataByYear(int fiscalPlanId, int year)
    {
        var categoryStatistics = await _unitOfWork.CategoriesRepository.GetDataByYear(fiscalPlanId, year);

        if(categoryStatistics.Count == 0)
        {
            return new FiscalPlanYearDTO();
        }

        var months = Enumerable.Range(1, 12);
        var grouped = categoryStatistics.SelectMany(a => a.Statistics)
                                        .GroupBy(s => s.Month);

        var monthlyOverspendingPerCategory = categoryStatistics.OrderBy(c => c.Category).Select(c => new MonthlyOverspendingPerCategory
        {
            Category = c.Category,
            OverspendingPerMonth = months.Select(month =>
                Math.Max(0, (c.Statistics.FirstOrDefault(s => s.Month == month)?.TotalSpent ?? 0)
                    - (c.BudgetLimits.LastOrDefault(bl => bl.Month.Month <= month)?.Budget ?? c.Budget)))
        }).ToList();

        Dictionary<int, MonthlyStatistic> statistics = [];

        decimal overspendingTotal = monthlyOverspendingPerCategory.SelectMany(s => s.OverspendingPerMonth).Sum();
        decimal totalSpent = 0;
        decimal happyEvaluatedTotal = 0;
        decimal unhappyEvaluatedTotal = 0;
        decimal necessaryEvaluatedTotal = 0;
        decimal unnecessaryEvaluatedTotal = 0;

        foreach (var group in grouped)
        {
            MonthlyStatistic monthlyStatistic = new()
            {
                Month = group.Key
            };

            foreach(var statistic in group)
            {
                monthlyStatistic.TotalSpent += statistic.TotalSpent;
                monthlyStatistic.HappyTransactions += statistic.HappyTransactions;
                monthlyStatistic.HappyEvaluatedTransactions += statistic.HappyEvaluatedTransactions;
                monthlyStatistic.UnhappyTransactions += statistic.UnhappyTransactions;
                monthlyStatistic.UnhappyEvaluatedTransactions += statistic.UnhappyEvaluatedTransactions;
                monthlyStatistic.NecessaryTransactions += statistic.NecessaryTransactions;
                monthlyStatistic.NecessaryEvaluatedTransactions += statistic.NecessaryEvaluatedTransactions;
                monthlyStatistic.UnnecessaryTransactions += statistic.UnnecessaryTransactions;
                monthlyStatistic.UnnecessaryEvaluatedTransactions += statistic.UnnecessaryEvaluatedTransactions;
                monthlyStatistic.UnevaluatedTransactions += statistic.UnevaluatedTransactions;

                totalSpent += statistic.TotalSpent;
                happyEvaluatedTotal+= statistic.HappyEvaluatedTransactions;
                unhappyEvaluatedTotal += statistic.UnhappyEvaluatedTransactions;
                necessaryEvaluatedTotal += statistic.NecessaryEvaluatedTransactions;
                unnecessaryEvaluatedTotal += statistic.UnnecessaryEvaluatedTransactions;
            }

            statistics[monthlyStatistic.Month] = monthlyStatistic;
        }


        var fiscalPlanDTO = new FiscalPlanYearDTO
        {
            TotalSpent = totalSpent,
            OverspendingTotal = overspendingTotal,
            HappyEvaluatedTotal = happyEvaluatedTotal,
            UnhappyEvaluatedTotal = unhappyEvaluatedTotal,
            NecessaryEvaluatedTotal = necessaryEvaluatedTotal,
            UnnecessaryEvaluatedTotal = unnecessaryEvaluatedTotal,
            TotalPerMonth = months.Select(month => statistics.TryGetValue(month, out var statistic) ? statistic.TotalSpent : 0).ToList(),
            HappyPerMonth = months.Select(month => statistics.TryGetValue(month, out var statistic) ? statistic.HappyTransactions : 0).ToList(),
            HappyEvaluatedPerMonth = months.Select(month => statistics.TryGetValue(month, out var statistic) ? statistic.HappyEvaluatedTransactions : 0).ToList(),
            UnhappyPerMonth = months.Select(month => statistics.TryGetValue(month, out var statistic) ? statistic.UnhappyTransactions : 0).ToList(),
            UnhappyEvaluatedPerMonth = months.Select(month => statistics.TryGetValue(month, out var statistic) ? statistic.UnhappyEvaluatedTransactions : 0).ToList(),
            NecessaryPerMonth = months.Select(month => statistics.TryGetValue(month, out var statistic) ? statistic.NecessaryTransactions : 0).ToList(),
            NecessaryEvaluatedPerMonth = months.Select(month => statistics.TryGetValue(month, out var statistic) ? statistic.NecessaryEvaluatedTransactions : 0).ToList(),
            UnnecessaryPerMonth = months.Select(month => statistics.TryGetValue(month, out var statistic) ? statistic.UnnecessaryTransactions : 0).ToList(),
            UnnecessaryEvaluatedPerMonth = months.Select(month => statistics.TryGetValue(month, out var statistic) ? statistic.UnnecessaryEvaluatedTransactions : 0).ToList(),
            UnevaluatedPerMonth = months.Select(month => statistics.TryGetValue(month, out var statistic) ? statistic.UnevaluatedTransactions : 0).ToList(),
            MonthlyOverspendingPerCategory = monthlyOverspendingPerCategory
        };    
        
        return fiscalPlanDTO;
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
