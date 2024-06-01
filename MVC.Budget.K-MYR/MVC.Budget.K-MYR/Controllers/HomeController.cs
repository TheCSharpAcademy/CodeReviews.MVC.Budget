using Microsoft.AspNetCore.Mvc;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Models.ViewModels;
using MVC.Budget.K_MYR.Services;
using System.Diagnostics;
using System.Globalization;

namespace MVC.Budget.K_MYR.Controllers;

public class HomeController : Controller
{
    private readonly IFiscalPlansService _fiscalPlanService;
    private readonly ICategoriesService _categorieService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IFiscalPlansService fiscalPlanService, ICategoriesService categorieService)
    {
        _logger = logger;
        _fiscalPlanService = fiscalPlanService;
        _categorieService = categorieService;
    }
    public async Task<IActionResult> Index()
    {
        return View(new LayoutModel<HomeModel>(new HomeModel
        {
            FiscalPlans = await _fiscalPlanService.GetFiscalPlans(),
            FiscalPlan = new()
        }, new CultureInfo("en-US"))); ;
    }

    [HttpGet("FiscalPlan/{id:int}")]
    public async Task<IActionResult> FiscalPlan([FromRoute]int id, DateTime? Month)
    {
        var fiscalPlanData = await _fiscalPlanService.GetDataByMonth(id, Month ?? DateTime.UtcNow);

        if (fiscalPlanData is null) 
        {
            return NotFound();
        }

        FiscalPlanModel fiscalPlanModel = new()
        {
            FiscalPlan = fiscalPlanData,        
            Category = new (),
            Transaction = new(),
            Search = new(),
        };
        return View(new LayoutModel<FiscalPlanModel>(fiscalPlanModel, new CultureInfo("en-US")));
    }    

    [HttpGet("Category/{id}")]
    public async Task<IActionResult> Category([FromRoute] int id)
    {
        var category = await _categorieService.GetByIDAsync(id);

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
