using Budget.Doc415.Data;
using Budget.Doc415.Models;
using Budget.Doc415.Services;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Budget.Doc415.Controllers;


public class TransactionController : Controller
{
    private readonly TransactionService _transactionService;
    private readonly CategoryService _categoryService;
    private readonly Seeder _seeder;
    private bool firstRun = true;

    public TransactionController(TransactionService transactionService, CategoryService categoryService, Seeder seeder)
    {
        _transactionService = transactionService;
        _categoryService = categoryService;
        _seeder = seeder;
    }


    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> Index(string SearchName, string StartDate, string EndDate, string FilterCategory)
    {

        _seeder.SeedDb();

        int? _filterCategoryID = null;
        if (string.IsNullOrEmpty(FilterCategory))
        {
            _filterCategoryID = null;
        }
        else
            _filterCategoryID = await _categoryService.GetCategoryByName(FilterCategory);

        var transactionList = await _transactionService.GetAllTransactions(SearchName, StartDate, EndDate, _filterCategoryID);
        transactionList = transactionList.OrderByDescending(x => x.Date).ToList();
        var categoryList = await _categoryService.GetAllCategories();
        var categoriesSelectList = await _categoryService.GetCategoriesForPullDown();
        var modelToView = new IndexViewModel()
        {
            Categories = categoryList,
            Transactions = transactionList,
            NewCategory = new Category() { Name = string.Empty },
            NewTransaction = new TransactionViewModel(),
            CategoriesSelect = categoriesSelectList
        };
        return View(modelToView);

    }


    [HttpPost]
    public async Task<ActionResult> Add(TransactionViewModel NewTransaction)
    {
        await _transactionService.InsertTransaction(NewTransaction);
        return RedirectToAction("Index", "Transaction");
    }


    [HttpPost]
    public async Task<IActionResult> Put(int id, TransactionViewModel transaction)
    {
        if (id != transaction.Id)
        {
            await Task.FromResult(result: BadRequest());
        }
        await _transactionService.UpdateTransaction(id, transaction);
        return RedirectToAction("Index", "Transaction");
    }


    [HttpPost]
    public async Task Delete(int id)
    {
        await _transactionService.DeleteTransaction(id);
    }
}
