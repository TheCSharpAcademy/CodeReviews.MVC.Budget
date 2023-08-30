using kmakai.Budget.Models;
using kmakai.Budget.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace kmakai.Budget.Controllers;

public class HomeController : Controller
{
    private readonly CategoryController _categoryController;
    private readonly TransactionController _transactionController;

    public HomeController(CategoryController categoryController, TransactionController transactionController)
    {
        _categoryController = categoryController;
        _transactionController = transactionController;
    }

    public IActionResult Index(HomeViewModel? model)
    {
        var transactions = Filter(model);

        var viewModel = new HomeViewModel
        {
            Categories = _categoryController.GetCategories(),
            Transactions = transactions.OrderByDescending(t => t.Date),
            TransactionTypes = _transactionController.GetTransactionTypes(),
            Balance = new BalanceViewModel
            {
                Income = _transactionController.GetTransactions().Where(t => t.TransactionTypeId == 1).Sum(t => t.Amount),
                Expense = _transactionController.GetTransactions().Where(t => t.TransactionTypeId == 2).Sum(t => t.Amount),
            }

        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult AddCategory(AddCategoryViewModel addCategory)
    {

        var category = new Category
        {
            Name = addCategory.Name
        };

        if (addCategory.Id != 0)
        {
            category.Id = addCategory.Id;
            _categoryController.UpdateCategory(category);
        }
        else
        {
            _categoryController.AddCategory(category);
        }

        return RedirectToAction("Index");
    }

    public IActionResult DeleteCategory(int id)
    {
        _categoryController.DeleteCategory(id);
        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult AddTransaction(AddTransactionViewModel addTransaction)
    {
        var transaction = new Transaction
        {
            Name = addTransaction.Name,
            Amount = addTransaction.Amount,
            Date = addTransaction.Date,
            CategoryId = addTransaction.CategoryId,
            TransactionTypeId = addTransaction.TransactionTypeId
        };

        if (addTransaction.Id != 0)
        {
            transaction.Id = addTransaction.Id;
            _transactionController.UpdateTransaction(transaction);
        }
        else
        {
            _transactionController.AddTransaction(transaction);
        }

        return RedirectToAction("Index");
    }

    public IActionResult DeleteTransaction(int id)
    {
        _transactionController.DeleteTransaction(id);
        return RedirectToAction("Index");
    }


    private List<Transaction> Filter(HomeViewModel? model)
    {
        var transactions = _transactionController.GetTransactions();
        if (model.FilterParams.CategoryId == null && model.FilterParams?.StartDate == null && model.FilterParams?.EndDate == null)
        {
            return transactions;
        }

        if (model.FilterParams.CategoryId != 0 && model.FilterParams.StartDate == null)
        {
            transactions = transactions.Where(t => t.CategoryId == model.FilterParams.CategoryId).ToList();
        }

        if (model.FilterParams.CategoryId == 0 && model.FilterParams.StartDate != null && model.FilterParams.EndDate == null)
        {
            transactions = transactions.Where(t => t.Date >= model.FilterParams.StartDate).ToList();
        }

        if (model.FilterParams.CategoryId == 0 && model.FilterParams.StartDate != null && model.FilterParams.EndDate != null)
        {
            transactions = transactions.Where(t => t.Date >= model.FilterParams.StartDate
            && t.Date <= model.FilterParams.EndDate).ToList();
        }

        if (model.FilterParams.CategoryId != 0 && model.FilterParams.StartDate != null)
        {
            transactions = transactions.Where(t => t.CategoryId == model.FilterParams.CategoryId
            && t.Date >= model.FilterParams.StartDate && t.Date <= model.FilterParams.EndDate).ToList();
        }

        return transactions;
    }

}