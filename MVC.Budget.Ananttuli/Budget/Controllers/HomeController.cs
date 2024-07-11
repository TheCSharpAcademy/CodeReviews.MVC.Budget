using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Budget.Models;
using Budget.CategoriesModule.Models;
using Budget.Data;
using Budget.TransactionsModule.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
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

    private async Task<TransactionsViewModel> GetTransactionsViewModel(TransactionsViewModelActiveTab? activeTab = null)
    {
        var transactions = await _db.Transactions.ToListAsync();
        var categories = await _db.Categories.ToListAsync();

        var model = new TransactionsViewModel
        {
            Transactions = transactions,
            Categories = categories,
            ActiveTab = activeTab ?? TransactionsViewModelActiveTab.Transactions
        };

        return model;
    }

    public async Task<IActionResult> Index(TransactionsViewModelActiveTab? activeTab)
    {
        return View(await GetTransactionsViewModel(activeTab));
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
