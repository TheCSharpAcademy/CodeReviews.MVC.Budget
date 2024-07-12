using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Budget.Data;
using Microsoft.EntityFrameworkCore;
using Budget.ViewModels;
using Budget.TransactionsModule.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BudgetDb _db;

    public HomeController(ILogger<HomeController> logger, BudgetDb budgetDb)
    {
        _logger = logger;
        _db = budgetDb;
    }

    private async Task<List<Transaction>> FindTransactions(string? name = null, int? categoryId = null)
    {
        IQueryable<Transaction> transactionQuery = _db.Transactions;

        if (name is not null)
        {
            transactionQuery = transactionQuery.Where(
                t => t.Description.ToLower().Contains(name.ToLower())
            );
        }

        if (categoryId is not null)
        {
            transactionQuery = transactionQuery.Where(
                t => t.CategoryId == categoryId
            );
        }

        return await transactionQuery.ToListAsync();
    }

    private async Task<HomeViewModel> GetTransactionsViewModel(
        TransactionsViewModelActiveTab? activeTab = null,
        string? searchText = null,
        int? searchCategoryId = null
    )
    {
        var transactions = await FindTransactions(searchText, searchCategoryId);
        var categories = await _db.Categories.ToListAsync();

        var transactionsList = new TransactionsListViewModel
        {
            Transactions = transactions,
            SearchCategoryId = searchCategoryId,
            SearchText = searchText,
            CategoriesList = new SelectList(categories, "Id", "Name", searchCategoryId)
        };

        var model = new HomeViewModel
        {
            TransactionList = transactionsList,
            Categories = categories,
            ActiveTab = activeTab ?? TransactionsViewModelActiveTab.Transactions
        };

        return model;
    }

    public async Task<IActionResult> Index(
        TransactionsViewModelActiveTab? activeTab,
        string? searchText = null,
        int? searchCategoryId = null
    )
    {
        return View(await GetTransactionsViewModel(activeTab, searchText, searchCategoryId));
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
