using Budget.Data;
using Budget.TransactionsModule.Models;
using Budget.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Budget.Controllers;

public class TransactionsController : Controller
{
    private readonly BudgetDb _db;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ILogger<TransactionsController> logger, BudgetDb budgetDb)
    {
        _logger = logger;
        _db = budgetDb;
    }


    public async Task<PartialViewResult> CreateTransactionModal()
    {
        var categoriesSelectList = await GetCategoriesSelectList();

        var model = new UpsertTransactionViewModel
        {
            Transaction = new Transaction { Date = DateTime.Today },
            AllCategories = categoriesSelectList
        };


        return PartialView("_PartialCreateTransactionModalView", model);
    }

    private async Task<SelectList> GetCategoriesSelectList()
    {
        var categories = await _db.Categories.ToListAsync();
        return new SelectList(categories, "Id", "Name");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<NoContent> CreateTransaction(
        [Bind("Transaction", "Transaction.Description", "Transaction.Date", "Transaction.Amount",
            "Transaction.CategoryId")]
        UpsertTransactionViewModel model
    )
    {
        _db.Transactions.Add(
            new Transaction
            {
                Description = model.Transaction.Description,
                Date = model.Transaction.Date,
                Amount = model.Transaction.Amount,
                CategoryId = model.Transaction.CategoryId
            }
        );

        await _db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public async Task<PartialViewResult> EditTransactionModal(int? id)
    {
        var transaction = await _db.Transactions.FindAsync(id);

        var model = new UpsertTransactionViewModel
        {
            Transaction = transaction,
            AllCategories = await GetCategoriesSelectList()
        };

        return PartialView("_PartialEditTransactionModalView", model);
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<Results<NotFound, Ok<Transaction>>> UpdateTransaction(
        [Bind("Transaction", "Transaction.Id", "Transaction.Description", "Transaction.Date", "Transaction.Amount",
            "Transaction.CategoryId")]
        UpsertTransactionViewModel model
    )
    {
        var foundTransaction = await _db.Transactions.FindAsync(model.Transaction.Id);

        if (foundTransaction is null) return TypedResults.NotFound();

        foundTransaction.Description = model.Transaction.Description;
        foundTransaction.Date = model.Transaction.Date;
        foundTransaction.Amount = model.Transaction.Amount;
        foundTransaction.CategoryId = model.Transaction.CategoryId;

        _db.Transactions.Update(foundTransaction);

        await _db.SaveChangesAsync();

        return TypedResults.Ok(foundTransaction);
    }


    [HttpDelete]
    public async Task<Results<NotFound, NoContent>> Delete(int id)
    {
        var existingTransaction = await _db.Transactions.FindAsync(id);

        if (existingTransaction is null) return TypedResults.NotFound();

        _db.Transactions.Remove(existingTransaction);

        await _db.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}