using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Budget.Models;
using Budget.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index(string? searchString, int? category, DateTime? date, int? id)
    {
        WalletCategoryTransactionViewModel viewModel;

        if (id is null or -1)
        {
            var wallets = await _unitOfWork.Wallets.GetEntities(_ => true);

            viewModel = new WalletCategoryTransactionViewModel
            {
                Wallets = new SelectList(wallets, "Id", "Name"),
                Wallet = await _unitOfWork.Wallets.GetEntity(id)
            };
        }
        else
        {
            viewModel = await GetViewModelFromId(id, searchString, category, date);
        }

        return View(viewModel);
    }

    private async Task<WalletCategoryTransactionViewModel> GetViewModelFromId(int? id, string? searchString,
        int? category, DateTime? date)
    {
        var wallets = await _unitOfWork.Wallets.GetEntities(_ => true);

        var wallet = await _unitOfWork.Wallets.GetEntity(id);

        if (wallet is null)
        {
            return new WalletCategoryTransactionViewModel();
        }

        var transactions = await GetTransactions(wallet.Id, searchString, category, date);

        var categories = await _unitOfWork.Categories.GetEntities(_ => true);

        var viewModel = new WalletCategoryTransactionViewModel
        {
            Wallets = new SelectList(wallets, "Id", "Name"),
            Wallet = wallet,
            WalletId = wallet.Id,
            CategoriesSelectList = new SelectList(categories, "Id", "Name"),
            Transactions = transactions
        };
        
        return viewModel;
    }

    private async Task<IEnumerable<Transaction>> GetTransactions(int? id, string? searchString, int? category,
        DateTime? date)
    {
        var transactions = await _unitOfWork.Transactions
            .GetTransactionsWithCategories(tr => tr.WalletId == id);

        if (transactions is null)
        {
            return Enumerable.Empty<Transaction>();
        }

        if (searchString is not null)
        {
            transactions = transactions.Where(tr => tr.Name.ToLower().Contains(searchString.ToLower()));
        }

        if (category is not null)
        {
            transactions = transactions.Where(tr => tr.CategoryId == category);
        }

        if (date is not null)
        {
            transactions = transactions.Where(tr => tr.Date?.Date == date.Value.Date);
        }

        return transactions;
    }

    [HttpPost]
    public async Task<IActionResult> AddWallet(Wallet wallet)
    {
        wallet.Balance = wallet.Income;
        
        await _unitOfWork.Wallets.AddEntity(wallet);
        await _unitOfWork.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateWallet(Wallet wallet)
    {
        var oldWallet = await _unitOfWork.Wallets.GetEntity(wallet.Id);

        if (oldWallet is null)
        {
            return Error();
        }
        
        oldWallet.Name = wallet.Name;

        var incomeDifference = wallet.Income - oldWallet.Income;

        if (incomeDifference != 0)
        {
            oldWallet.Balance += incomeDifference;
        }
        
        oldWallet.Income = wallet.Income;

        await _unitOfWork.Wallets.Update(oldWallet.Id, oldWallet);

        await _unitOfWork.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteWallet(Wallet wallet)
    {
        await _unitOfWork.Wallets.RemoveEntity(wallet.Id);

        await _unitOfWork.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddTransaction(Transaction transaction, int walletId)
    {
        transaction.Date = DateTime.Now;
        transaction.WalletId = walletId;

        await _unitOfWork.Transactions.AddEntity(transaction);

        var wallet = await _unitOfWork.Wallets.GetEntity(walletId);

        if (wallet is not null)
        {
            wallet.Expenses += transaction.Cost;
            wallet.Balance -= transaction.Cost;
            
            await _unitOfWork.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTransaction(Transaction transaction)
    {
        var formerTransaction = await _unitOfWork.Transactions.GetEntity(transaction.Id);

        var costDifference = transaction.Cost - formerTransaction?.Cost;

        transaction.Date = formerTransaction?.Date;

        await _unitOfWork.Transactions.Update(transaction.Id, transaction);

        var wallet = await _unitOfWork.Wallets.GetEntity(transaction.WalletId);

        if (wallet is not null)
        {
            if (costDifference != 0)
            {
                wallet.Expenses += costDifference;
                wallet.Balance -= costDifference;
            }

            await _unitOfWork.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTransaction(Transaction transaction)
    {
        await _unitOfWork.Transactions.RemoveEntity(transaction.Id);

        var wallet = await _unitOfWork.Wallets.GetEntity(transaction.WalletId);

        if (wallet is not null)
        {
            wallet.Expenses -= transaction.Cost;
            wallet.Balance += transaction.Cost;

            await _unitOfWork.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}