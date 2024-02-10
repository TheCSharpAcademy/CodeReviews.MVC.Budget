using Microsoft.AspNetCore.Mvc;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;
using System.Diagnostics;

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
        HomeViewModel HomeViewModel = new()
        {
            Income = await _unitOfWork.CategoriesRepository.GetAsync(c => c.GroupId == 1, q => q.OrderBy(t => t.Name)),
            Expenses = await _unitOfWork.CategoriesRepository.GetAsync(c => c.GroupId == 2, q => q.OrderBy(t => t.Name)),
            Savings = await _unitOfWork.CategoriesRepository.GetAsync(c => c.GroupId == 3, q => q.OrderBy(t => t.Name)),
            Category = new()
        };

        return View(HomeViewModel);
    }

    [HttpGet("Category/{id}")]
    public async Task<IActionResult> Category([FromRoute] int id)
    {
        var category = await _unitOfWork.CategoriesRepository.GetCategoryAsync(id);
        return View(category);
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
