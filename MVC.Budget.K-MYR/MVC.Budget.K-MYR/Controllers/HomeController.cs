using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Models.ViewModels;
using MVC.Budget.K_MYR.Services;
using System.Diagnostics;
using System.Globalization;

namespace MVC.Budget.K_MYR.Controllers;

public class HomeController(ILogger<HomeController> logger, IFiscalPlansService fiscalPlanService, ICategoriesService categorieService) : Controller
{
    private readonly IFiscalPlansService _fiscalPlanService = fiscalPlanService;
    private readonly ICategoriesService _categorieService = categorieService;
    private readonly ILogger<HomeController> _logger = logger;

    [HttpPost("Country")]
    [ValidateAntiForgeryToken]
    public IActionResult SetCountry([FromBody] string countryISOCode)
    {
        RegionInfo regionInfo;

        try
        {
            regionInfo = new RegionInfo("en-" + countryISOCode);
        }
        catch (ArgumentException)
        {
            return BadRequest($"'{countryISOCode}' is not a valid country ISO Code.");
        }

        var options = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddYears(1),
            Secure = true,
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict
        };

        var currency = regionInfo.ISOCurrencySymbol == "XXX" ? "USD" : regionInfo.ISOCurrencySymbol;

        var preferences = new UserPreferences
        {
            Locale = $"en-{regionInfo.Name}",
            Currency = currency
        };

        Response.Cookies.Append("Locale", $"en-{regionInfo.Name}", options);

        return Ok(preferences);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var (culture, currency) = GetUserPreferences();

        var homeModel = new IndexViewModel
        {
            FiscalPlans = await _fiscalPlanService.GetFiscalPlanDTOs(),
            FiscalPlan = new(),
        };

        LayoutModel<IndexViewModel> viewModel = new(homeModel, culture, currency);

        return View(viewModel);
    }

    [HttpGet("FiscalPlan/{id:int}")]
    public async Task<IActionResult> FiscalPlan([FromRoute]int id, [FromQuery] DateTime? Month)
    {
        var fiscalPlan = await _fiscalPlanService.GetByIDAsync(id);

        if (fiscalPlan is null)
        {
            return NotFound();
        }

        var fiscalPlanDTO = await _fiscalPlanService.GetDataByMonth(fiscalPlan, Month ?? DateTime.UtcNow);
        var categories = await _categorieService.GetCategoriesWithUnevaluatedTransactions(id, 10);
        var (culture, currency) = GetUserPreferences();
        var selectList = new SelectList(fiscalPlanDTO.ExpenseCategories.Concat(fiscalPlanDTO.IncomeCategories)
                                                                       .OrderBy(c => c.Name)
                                                                       .ToList(), "Id", "Name");

        FiscalPlanViewModel fiscalPlanModel = new()
        {
            FiscalPlan = fiscalPlanDTO,     
            Categories = categories,
            Category = new (),
            Transaction = new(),
            Search = new()
            {
                FiscalPlanId = id,
                Categories = selectList
            },
        };

        LayoutModel<FiscalPlanViewModel> viewModel = new(fiscalPlanModel, culture, currency);

        return View(viewModel);
    }    

    [HttpGet("Category/{id}")]
    public async Task<IActionResult> Category([FromRoute] int id, [FromQuery] DateTime? Month)
    {
        var category = await _categorieService.GetCategoryDataByMonth(id, Month ?? DateTime.UtcNow);

        if (category is null)
            return NotFound();

        var (culture, currency) = GetUserPreferences();

        CategoryViewModel categoryModel = new()
        {
            Category = category,
            Transaction = new(),
        };

        LayoutModel <CategoryViewModel> viewModel = new(categoryModel, culture, currency);

        return View(viewModel);
    }
   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private (CultureInfo culture, string currency) GetUserPreferences()
    {
        RegionInfo regionInfo = new("en-US");

        if (Request.Cookies.TryGetValue("Locale", out var locale))
        {
            try
            {
                regionInfo = new RegionInfo(locale);                
            }
            catch (Exception)
            {
                var options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(-2),
                    Secure = true,
                    HttpOnly = true,
                    IsEssential = true,
                };

                Response.Cookies.Append("Locale", "", options);
            }
        }

        var culture = new CultureInfo("en-" + regionInfo.Name);
        culture.NumberFormat.CurrencyPositivePattern = 2;
        var currency = regionInfo.ISOCurrencySymbol == "XXX" ? "USD" : regionInfo.ISOCurrencySymbol;

        return (culture, currency);
    }
}
