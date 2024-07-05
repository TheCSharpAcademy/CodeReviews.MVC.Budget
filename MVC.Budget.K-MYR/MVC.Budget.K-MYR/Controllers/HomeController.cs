using Microsoft.AspNetCore.Mvc;
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
            return BadRequest();
        }

        var options = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddYears(1),
            Secure = true,
            HttpOnly = true,
            IsEssential = true,
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
        var regionInfo = GetRegionInfo();        
        var culture = new CultureInfo("en-" + regionInfo.Name);
        culture.NumberFormat.CurrencyPositivePattern = 2;
        var currency = regionInfo.ISOCurrencySymbol == "XXX" ? "USD" : regionInfo.ISOCurrencySymbol;

        var homeModel = new HomeModel
        {
            FiscalPlans = await _fiscalPlanService.GetFiscalPlans(),
            FiscalPlan = new()
        };

        LayoutModel<HomeModel> viewModel = new(homeModel, culture, currency);

        return View(viewModel);
    }

    [HttpGet("FiscalPlan/{id:int}")]
    public async Task<IActionResult> FiscalPlan([FromRoute]int id, DateTime? Month)
    {
        var fiscalPlan = await _fiscalPlanService.GetByIDAsync(id);

        if (fiscalPlan is null)
        {
            return NotFound();
        }

        var fiscalPlanData = await _fiscalPlanService.GetDataByMonth(fiscalPlan, Month ?? DateTime.UtcNow);

        var regionInfo = GetRegionInfo();
        var culture = new CultureInfo("en-" + regionInfo.Name);
        culture.NumberFormat.CurrencyPositivePattern = 2;
        var currency = regionInfo.ISOCurrencySymbol == "XXX" ? "USD" : regionInfo.ISOCurrencySymbol;

        FiscalPlanModel fiscalPlanModel = new()
        {
            FiscalPlan = fiscalPlanData,        
            Category = new (),
            Transaction = new(),
            Search = new(),
        };

        LayoutModel<FiscalPlanModel> viewModel = new(fiscalPlanModel, culture, currency);

        return View(viewModel);
    }    

    [HttpGet("Category/{id}")]
    public async Task<IActionResult> Category([FromRoute] int id)
    {
        var category = await _categorieService.GetByIDAsync(id);

        if (category is null)
            return NotFound();

        var regionInfo = GetRegionInfo();
        var culture = new CultureInfo("en-" + regionInfo.Name);
        culture.NumberFormat.CurrencyPositivePattern = 2;
        var currency = regionInfo.ISOCurrencySymbol == "XXX" ? "USD" : regionInfo.ISOCurrencySymbol;

        return View(new LayoutModel<Category>(category, culture, currency));
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

    private RegionInfo GetRegionInfo()
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
        
        return regionInfo;
    }
}
