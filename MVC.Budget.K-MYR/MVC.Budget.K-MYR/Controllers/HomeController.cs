using Microsoft.AspNetCore.Mvc;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;
using System.Diagnostics;
using System.Globalization;

namespace MVC.Budget.K_MYR.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        HomeModel HomeModel = new()
        {
            Income = await _unitOfWork.CategoriesRepository.GetCategoriesWithFilteredTransactionsAsync(
                c => c.GroupId == 1, 
                q => q.OrderBy(t => t.Name), 
                c => c.Transactions.Where(t => t.DateTime.Year == DateTime.UtcNow.Year && t.DateTime.Month == DateTime.UtcNow.Month)
                    .OrderByDescending(d => d.DateTime)),
            Expenses = await _unitOfWork.CategoriesRepository.GetCategoriesWithFilteredTransactionsAsync(
                c => c.GroupId == 2,
                q => q.OrderBy(t => t.Name),
                c => c.Transactions.Where(t => t.DateTime.Year == DateTime.UtcNow.Year && t.DateTime.Month == DateTime.UtcNow.Month)
                    .OrderByDescending(d => d.DateTime)),
            Savings = await _unitOfWork.CategoriesRepository.GetCategoriesWithFilteredTransactionsAsync(
                c => c.GroupId == 3,
                q => q.OrderBy(t => t.Name),
                c => c.Transactions.Where(t => t.DateTime.Year == DateTime.UtcNow.Year && t.DateTime.Month == DateTime.UtcNow.Month)
                    .OrderByDescending(d => d.DateTime)),
            Category = new(),
            Transaction = new(),
            Search = new(),
        };

        return View(new LayoutModel<HomeModel>(HomeModel, new CultureInfo("en-US")));
    }

    [HttpGet("Category/{id}")]
    public async Task<IActionResult> Category([FromRoute] int id)
    {
        var category = await _unitOfWork.CategoriesRepository.GetCategoryAsync(id);

        if (category is null)
            return NotFound();

        return View(new LayoutModel<Category>(category, new CultureInfo("en-US")));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
