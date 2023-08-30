using kmakai.Budget.Repositories;
using Microsoft.AspNetCore.Mvc;
using kmakai.Budget.Models;

namespace kmakai.Budget.Controllers;

public class TransactionController : Controller
{
   private readonly IRepository _repository;

    public TransactionController(IRepository repository)
    {
        _repository = repository;
    }

    public List<Transaction> GetTransactions()
    {
        return _repository.GetTransactions();
    }

    public List<TransactionType> GetTransactionTypes()
    {
        return _repository.GetTransactionTypes();
    }

    public void AddTransaction(Transaction transaction)
    {
        _repository.AddTransaction(transaction);
    }

    public void UpdateTransaction(Transaction transaction)
    {
        _repository.UpdateTransaction(transaction);
    }

    public void DeleteTransaction(int id)
    {
        _repository.DeleteTransaction(id);
    }
}
