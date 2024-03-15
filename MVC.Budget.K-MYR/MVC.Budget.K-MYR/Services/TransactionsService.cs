using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;
using System.Linq.Expressions;

namespace MVC.Budget.K_MYR.Services;

public class TransactionsService : ITransactionsService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionsService> _logger;

    public TransactionsService(IUnitOfWork unitOfWork, ILogger<TransactionsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Task<List<Transaction>> GetTransactions()
    {
        return _unitOfWork.TransactionsRepository.GetAsync();
    }

    public ValueTask<Transaction?> GetByIDAsync(int id)
    {
        return _unitOfWork.TransactionsRepository.GetByIdAsync(id);
    }

    public Transaction? GetByID(int id)
    {
        return _unitOfWork.TransactionsRepository.GetByID(id);
    }

    public async Task<Transaction> AddTransaction(TransactionPost transactionPost, Category category)
    {
        var transaction = new Transaction()
        {
            Title = transactionPost.Title,
            DateTime = transactionPost.DateTime,
            Amount = transactionPost.Amount,
            IsHappy = transactionPost.IsHappy,
            IsNecessary = transactionPost.IsNecessary,
            CategoryId = category.Id,
        };

        _unitOfWork.TransactionsRepository.Insert(transaction);

        var statistic = category.Statistics.SingleOrDefault();

        if (statistic is null)
        {
            _unitOfWork.CategoryStatisticsRepository.Insert(new CategoryStatistic
            {
                CategoryId = category.Id,
                Budget = category.Budget,
                TotalSpent = transaction.Amount,
                Month = transaction.DateTime
            });
        }
        else
        {
            statistic.TotalSpent += transaction.Amount;
            statistic.Budget = category.Budget;
            statistic.Overspending = Math.Max(0, statistic.TotalSpent - statistic.Budget);

            _unitOfWork.CategoryStatisticsRepository.Update(statistic);
        }

        await _unitOfWork.Save();

        return transaction;
    }

    public async Task UpdateTransaction(Category category, Transaction transaction, TransactionPut transactionPut)
    {
        if(transaction.DateTime.Month != transactionPut.DateTime.Month || transaction.DateTime.Year != transactionPut.DateTime.Year || transaction.CategoryId != transactionPut.CategoryId)
        {
            var oldStatistic = await _unitOfWork.CategoryStatisticsRepository.GetByCategoryIdAndDateTimeAsync(transaction.CategoryId, transaction.DateTime);

            if(oldStatistic is not null)
            {
                oldStatistic.ChangeTotalSpent(-transaction.Amount);
                _unitOfWork.CategoryStatisticsRepository.Update(oldStatistic);
            }
        }

        var statistic = category.Statistics.SingleOrDefault();

        if (statistic is null)
        {
            _unitOfWork.CategoryStatisticsRepository.Insert(new CategoryStatistic
            {
                CategoryId = category.Id,
                Budget = category.Budget,
                TotalSpent = transactionPut.Amount,
                Month = DateTime.UtcNow
            });
        }
        else
        {
            statistic.ChangeTotalSpent(transactionPut.Amount - transaction.Amount);
            _unitOfWork.CategoryStatisticsRepository.Update(statistic);
        }

        transaction.Title = transactionPut.Title;
        transaction.Description = transactionPut.Description;
        transaction.Amount = transactionPut.Amount;
        transaction.IsHappy = transactionPut.IsHappy;
        transaction.IsNecessary = transactionPut.IsNecessary;
        transaction.DateTime = transactionPut.DateTime;
        transaction.CategoryId = transactionPut.CategoryId;
        transaction.Evaluated = transactionPut.Evaluated;
        transaction.PreviousIsNecessary = transactionPut.PreviousIsNecessary;
        transaction.PreviousIsHappy = transactionPut.PreviousIsHappy;


        _unitOfWork.TransactionsRepository.Update(transaction);

        await _unitOfWork.Save();
    }

    public async Task DeleteTransaction(Transaction transaction)
    {
        var statistic = await _unitOfWork.CategoryStatisticsRepository.GetByCategoryIdAndDateTimeAsync(transaction.CategoryId, transaction.DateTime);

        if (statistic != null)
        {
            statistic.ChangeTotalSpent(-transaction.Amount);
            _unitOfWork.CategoryStatisticsRepository.Update(statistic);
        }

        _unitOfWork.TransactionsRepository.Delete(transaction);
        await _unitOfWork.Save();
    }

    public Task<Category?> GetCategoryWithFilteredStatistics(int id, Expression<Func<CategoryStatistic, bool>> filter)
    {
        return _unitOfWork.CategoriesRepository.GetCategoryWithFilteredStatistics(id, filter);
    }
}


