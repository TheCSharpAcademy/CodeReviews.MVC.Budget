using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Budget.JsPeanut.Data;
using MVC.Budget.JsPeanut.Models;
using MVC.Budget.JsPeanut.Models.ViewModel;
using MVC.Budget.JsPeanut.Services;
using System.Text.Json;

namespace MVC.Budget.JsPeanut.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly CategoryService _categoryService;
        private readonly TransactionService _transactionService;
        private readonly JsonFileCurrencyService _jsonFileCurrencyService;
        private readonly CurrencyConverterService _currencyConverterService;
        public CategoriesController(DataContext context, CategoryService categoryService, TransactionService transactionService, JsonFileCurrencyService jsonFileCurrencyService, CurrencyConverterService currencyConverterService)
        {
            _context = context;
            _categoryService = categoryService;
            _transactionService = transactionService;
            _jsonFileCurrencyService = jsonFileCurrencyService;
            _currencyConverterService = currencyConverterService;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            var transactions = _context.Transactions.ToList();
            var currencies = _jsonFileCurrencyService.GetCurrencyList();
            var categoryselectlist_ = new List<SelectListItem>();
            var currencyselectlist_ = new List<SelectListItem>();
            foreach (var category in categories)
            {
                categoryselectlist_.Add(new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }
            foreach (var currency in currencies)
            {
                currencyselectlist_.Add(new SelectListItem
                {
                    Text = $"{currency.Name} ({currency.CurrencyCode})",
                    Value = /*currency.NativeSymbol */JsonSerializer.Serialize(currency)
                });
            }
            var categoriesviewmodel = new CategoryViewModel
            {
                Categories = categories,
                CategorySelectList = categoryselectlist_,
                CurrencySelectList = currencyselectlist_,
                Transactions = transactions
            };
            Currency currencyObject = new Currency
            {
                CurrencyCode = categories.First().CurrencyCode,
                NativeSymbol = categories.First().CurrencyNativeSymbol,
                Name = ""
            };
            ViewBag.Currency = currencyObject.CurrencyCode;
            var currencyObjectJson = JsonSerializer.Serialize<Currency>(currencyObject);
            CategoryViewModel updatecurrencycvm = new CategoryViewModel
            {
                CurrencyObjectJson = currencyObjectJson
            };
            UpdateCurrency(updatecurrencycvm);

            return View(categoriesviewmodel);
        }

        [HttpPost]
        public IActionResult UpdateCurrency(CategoryViewModel cvm)
        {
            var categories = _categoryService.GetAllCategories();
            var transactions = _transactionService.GetAllTransactions();
            var selectedCurrencyOption = JsonSerializer.Deserialize<Currency>(cvm.CurrencyObjectJson);
            foreach (var category in categories)
            {
                var updatedCategory = category;
                updatedCategory.CurrencyCode = selectedCurrencyOption.CurrencyCode;
                updatedCategory.CurrencyNativeSymbol = selectedCurrencyOption.NativeSymbol;

                decimal totalValue = 0;

                var transactionsWhereCategoryIsEqualToLoopsCategory = transactions.Where(x => x.CategoryId == category.Id);
                foreach (var transaction in transactionsWhereCategoryIsEqualToLoopsCategory)
                {
                    if (transaction.CurrencyCode == categories.First().CurrencyCode)
                    {
                        totalValue += transaction.Value;
                    }
                    else
                    {
                        decimal conversionResult = _currencyConverterService.ConvertValueToCategoryCurrency(transaction.CurrencyCode, transaction.Value, categories.First().CurrencyCode);

                        totalValue += conversionResult;
                    }
                }

                totalValue = Decimal.Round(totalValue, 2);

                updatedCategory.TotalValue = totalValue;

                _categoryService.UpdateCategory(updatedCategory);
            }

            return Redirect("https://localhost:7229");
        }

        public IActionResult AddTransaction(Models.Transaction transaction, CategoryViewModel cvm)
        {
            var categories = _categoryService.GetAllCategories();
            var transactions = _transactionService.GetAllTransactions();

            var transactionCategory = categories.Where(c => c.Id == transaction.CategoryId).First();

            transactionCategory.TotalValue += transaction.Value;

            var selectedCurrencyOption = JsonSerializer.Deserialize<Currency>(cvm.CurrencyObjectJson);

            transaction.CurrencyCode = selectedCurrencyOption.CurrencyCode;
            transaction.CurrencyNativeSymbol = selectedCurrencyOption.NativeSymbol;

            _transactionService.AddTransaction(transaction);
            _context.SaveChanges();

            Currency currencyObject = new Currency
            {
                CurrencyCode = categories[0].CurrencyCode,
                NativeSymbol = categories[0].CurrencyNativeSymbol,
                Name = ""
            };
            string currencyJson = JsonSerializer.Serialize(currencyObject);
            CategoryViewModel cmv = new CategoryViewModel
            {
                CurrencyObjectJson = currencyJson
            };
            UpdateCurrency(cmv);

            return Redirect("https://localhost:7229");
        }
    }
}
