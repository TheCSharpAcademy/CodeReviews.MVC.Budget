using Microsoft.AspNetCore.Mvc;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Repositories;
using System.Diagnostics;

namespace MVC.Budget.K_MYR.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoriesRepository _repo;

    public HomeController(ILogger<HomeController> logger, ICategoriesRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<IActionResult> Index()
    {

        HomeViewModel HomeViewModel = new()
        {
            Income = await _repo.GetCategoriesAsync(c => c.GroupId == 1, q => q.OrderBy(t => t.Name)),
            Expenses = await _repo.GetCategoriesAsync(c => c.GroupId == 2, q => q.OrderBy(t => t.Name)),
            Savings = await _repo.GetCategoriesAsync(c => c.GroupId == 3, q => q.OrderBy(t => t.Name)),
            Category = new()
        };

        return View(HomeViewModel);
    }

    [HttpGet("Category/{id}")]
    public async Task<IActionResult> Category([FromRoute] int id)
    {
        return View();
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
