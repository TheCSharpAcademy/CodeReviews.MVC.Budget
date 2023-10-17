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

        public IActionResult Index(string timeline = "", string searchStringOne = "", string searchStringTwo = "")
        {
            var categories = _categoryService.GetAllCategories();
            var transactions = _transactionService.GetAllTransactions();

            if (!string.IsNullOrEmpty(timeline))
            {
                DateTime firstDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime lastDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                DateTime firstDayOfThreeMonthsAgoMonth = new DateTime(DateTime.Now.AddMonths(-3).Year, DateTime.Now.AddMonths(-3).Month, 1);
                DateTime lastDayOfThreeMonthsAgoMonth = new DateTime(DateTime.Now.AddMonths(-3).Year, DateTime.Now.AddMonths(-3).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-3).Year, DateTime.Now.AddMonths(-3).Month));

                DateTime firstDayOfSixMonthsAgoMonth = new DateTime(DateTime.Now.AddMonths(-6).Year, DateTime.Now.AddMonths(-6).Month, 1);
                DateTime lastDayOfSixMonthsAgoMonth = new DateTime(DateTime.Now.AddMonths(-6).Year, DateTime.Now.AddMonths(-6).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-6).Year, DateTime.Now.AddMonths(-6).Month));

                DateTime firstDayOfTLastYearMonth = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month, 1);
                DateTime lastDayOfLastYearMonth = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month));
                switch (timeline)
                {
                    case "LastMonth":
                        transactions = transactions = transactions.Where(x => x.Date.Date >= firstDayOfTheMonth && x.Date.Date <= lastDayOfTheMonth).ToList();
                        break;
                    case "LastThreeMonths":
                        transactions = transactions = transactions.Where(x => x.Date.Date >= firstDayOfThreeMonthsAgoMonth && x.Date.Date <= lastDayOfTheMonth).ToList();
                        break;
                    case "LastSixMonths":
                        transactions = transactions = transactions.Where(x => x.Date.Date >= firstDayOfSixMonthsAgoMonth && x.Date.Date <= lastDayOfTheMonth).ToList();
                        break;
                    case "LastYear":
                        transactions = transactions = transactions.Where(x => x.Date.Date >= firstDayOfTLastYearMonth && x.Date.Date <= lastDayOfTheMonth).ToList();
                        break;
                    case "default":
                        transactions = _transactionService.GetAllTransactions();
                        break;
                }
            }
            if (!string.IsNullOrEmpty(searchStringOne) && !string.IsNullOrEmpty(searchStringTwo))
            {
                DateTime searchDateOne;
                DateTime searchDateTwo;
                if (DateTime.TryParse(searchStringOne, out searchDateOne) && DateTime.TryParse(searchStringTwo, out searchDateTwo))
                {
                    transactions = transactions.Where(x => x.Date.Date >= searchDateOne.Date && x.Date.Date <= searchDateTwo.Date).ToList();
                }
            }
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
                    Value = JsonSerializer.Serialize<Currency>(currency)
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
                Name = string.Empty
            };
            ViewBag.Currency = currencyObject.CurrencyCode;
            var currencyObjectJson = JsonSerializer.Serialize<Currency>(currencyObject);
            UpdateCurrency(currencyObjectJson, transactions);

            return View(categoriesviewmodel);
        }

        [HttpPost]
        public IActionResult UpdateCurrency(string selectedCurrency, List<Transaction> sortedTransactions = null)
        {
            var categories = _categoryService.GetAllCategories();
            List<Transaction> transactions = new();
            if (sortedTransactions != null)
            {
                transactions = sortedTransactions;
            }
            else
            {
                transactions = _transactionService.GetAllTransactions();
            }
            var selectedCurrencyOption = JsonSerializer.Deserialize<Currency>(selectedCurrency);
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
                CurrencyCode = categories.First().CurrencyCode,
                NativeSymbol = categories.First().CurrencyNativeSymbol,
                Name = string.Empty
            };
            string currencyJson = JsonSerializer.Serialize(currencyObject);
            UpdateCurrency(currencyJson);

            return Redirect("https://localhost:7229");
        }
    }
}
