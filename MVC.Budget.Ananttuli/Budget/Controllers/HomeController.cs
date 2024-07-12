using System.Diagnostics;
using Budget.Data;
using Budget.TransactionsModule.Models;
using Budget.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Budget.Controllers;

public class HomeController : Controller
{
    private readonly BudgetDb _db;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, BudgetDb budgetDb)
    {
        _logger = logger;
        _db = budgetDb;
    }


    public async Task<IActionResult> Index(
        int? pageNumber,
        int? pageSize,
        TransactionsViewModelActiveTab? activeTab,
        string? searchText = null,
        int? searchCategoryId = null,
        DateOnly? startDateRange = null,
        DateOnly? endDateRange = null
    )
    {
        return View(await GetTransactionsViewModel(activeTab, searchText, searchCategoryId, pageNumber, pageSize,
            startDateRange, endDateRange));
    }


    private async Task<HomeViewModel> GetTransactionsViewModel(
        TransactionsViewModelActiveTab? activeTab = null,
        string? searchText = null,
        int? searchCategoryId = null,
        int? pageNumber = null,
        int? pageSize = null,
        DateOnly? startDateRange = null,
        DateOnly? endDateRange = null
    )
    {
        var validPageNumber = !pageNumber.HasValue || pageNumber < 1 ? 1 : pageNumber.Value;
        var validPageSize = !pageSize.HasValue || pageSize < 1 ? 20 : pageSize.Value;

        var (transactions, total) = await FindTransactions(
            validPageNumber, validPageSize, searchText,
            searchCategoryId, startDateRange, endDateRange
        );

        var categories = await _db.Categories.ToListAsync();

        var remainder = total % validPageSize;
        var totalPages = total / validPageSize + (remainder > 0 ? 1 : 0);

        var transactionsListViewModel = new TransactionsListViewModel
        {
            Transactions = transactions,
            Total = total,
            SearchCategoryId = searchCategoryId,
            SearchText = searchText,
            CategoriesList = new SelectList(categories, "Id", "Name", searchCategoryId),
            PageSize = validPageSize,
            PageNumber = validPageNumber,
            TotalPages = totalPages,
            PageNumbersList = new SelectList(Enumerable.Range(1, totalPages).ToList(), validPageNumber),
            StartDateRange = startDateRange,
            EndDateRange = endDateRange
        };

        var model = new HomeViewModel
        {
            TransactionList = transactionsListViewModel,
            Categories = categories,
            ActiveTab = activeTab ?? TransactionsViewModelActiveTab.Transactions
        };

        return model;
    }

    private async Task<(List<Transaction>, int)> FindTransactions(
        int pageNumber,
        int pageSize,
        string? name = null,
        int? categoryId = null,
        DateOnly? startDateRange = null,
        DateOnly? endDateRange = null
    )
    {
        if (pageSize < 1) return ([], 0);

        IQueryable<Transaction> transactionQuery = _db.Transactions;

        if (name is not null)
            transactionQuery = transactionQuery.Where(
                t => t.Description.ToLower().Contains(name.ToLower())
            );

        if (categoryId is not null)
            transactionQuery = transactionQuery.Where(
                t => t.CategoryId == categoryId
            );

        if (startDateRange.HasValue)
            transactionQuery = transactionQuery.Where(
                t => DateOnly.FromDateTime(t.Date) >= startDateRange.Value
            );

        if (endDateRange.HasValue)
            transactionQuery = transactionQuery.Where(
                t => DateOnly.FromDateTime(t.Date) <= endDateRange.Value
            );

        var total = await transactionQuery.CountAsync();

        var transactions = await transactionQuery
            .OrderByDescending(t => t.Date)
            .ThenBy(t => t.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync();

        return (transactions, total);
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}