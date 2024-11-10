using Budget.Doc415.Models;
using Budget.Doc415.Repositories;
using Budget.Doc415.Transformations;

namespace Budget.Doc415.Services;

public class TransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    public TransactionService(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }


    public async Task<List<TransactionViewModel>> GetAllTransactions(string SearchName, string StartDate, string EndDate, int? FilterCategory)
    {
        var _startDate = Transformer.ConvertToDateTime(StartDate);
        var _endDate = Transformer.ConvertToDateTime(EndDate);
        var transactionsFromDb = await _transactionRepository.GetAllTransactions(SearchName, _startDate, _endDate, FilterCategory);
        var categories = await _categoryRepository.GetAllCategories();
        var transactionsToView = Transformer.ConvertToTransactionVM(transactionsFromDb, categories);
        return transactionsToView;

    }

    public async Task InsertTransaction(TransactionViewModel transactionVM)
    {
        var categories = await _categoryRepository.GetAllCategories();
        Transaction transactionToDb = Transformer.ConvertToDbTransaction(transactionVM, categories);
        await _transactionRepository.InsertTransaction(transactionToDb);
    }

    public async Task DeleteTransaction(int id)
    {
        await _transactionRepository.DeleteTransaction(id);
    }

    public async Task UpdateTransaction(int id, TransactionViewModel transactionVM)
    {
        var categories = await _categoryRepository.GetAllCategories();
        var transactionToDb = Transformer.ConvertToDbTransaction(transactionVM, categories);
        await _transactionRepository.UpdateTransaction(id, transactionToDb);
    }

    public async Task<Transaction> GetTransactionById(int id)
    {
        return await _transactionRepository.GetTransactionById(id);
    }

    public bool IsSeeded()
    {
        return _transactionRepository.IsSeeded();
    }

}
