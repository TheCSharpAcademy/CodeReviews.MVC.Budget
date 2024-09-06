using System.Linq.Dynamic.Core;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Extensions;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Enums;

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

    public async Task<TransactionsSearchResponse>? GetTransactions(TransactionsSearchRequest requestModel)
    {
        Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>? orderBy = q => q.OrderBy("Id ASC");
        Func<IQueryable<Transaction>, IQueryable<Transaction>>? filter = null;


        if (requestModel.OrderBy is not null && OrderingHelpers.IsAllowedProperty(requestModel.OrderBy))
        {
            string orderDirection = (requestModel.OrderDirection == OrderDirection.Ascending) ^ requestModel.IsPrevious ? "" : " DESC";
            string orderString = $"{requestModel.OrderBy} {orderDirection}, Id";
            orderBy = q => q.OrderBy(orderString);

            if (requestModel.LastId is not null && requestModel.LastValue is not null)
            {
                var type = OrderingHelpers.GetProperty<Transaction>(requestModel.OrderBy)?.PropertyType;

                if(type is not null)
                {
                    string comparerSymbol = requestModel.IsPrevious ? "<" : ">";
                    object lastValue = requestModel.LastValue;

                    if (requestModel.LastValue.GetType() != type)
                    {
                        try
                        {
                            lastValue = Convert.ChangeType(requestModel.LastValue, type);

                        }
                        catch
                        {
                            return null!;
                        }
                    }

                    filter = q => q.Where($"{requestModel.OrderBy} {comparerSymbol} @0 || ({requestModel.OrderBy} == @0 && Id {comparerSymbol} @1)",
                        lastValue, requestModel.LastId);
                }
            }
        }


        var searchModel = new TransactionsSearch()
        {
            PageSize = requestModel.PageSize + 1,
            SearchString = requestModel.SearchString,
            MinAmount = requestModel.MinAmount,
            MaxAmount = requestModel.MaxAmount,
            MinDate = requestModel.MinDate,
            MaxDate = requestModel.MaxDate,
            FiscalPlanId = requestModel.FiscalPlanId,
            CategoryId = requestModel.CategoryId,
        };

        var transactions = await _unitOfWork.TransactionsRepository.GetAsync(searchModel, orderBy, filter);
        var hasNext = transactions.Count > requestModel.PageSize;

        if (hasNext)
        {
            transactions.RemoveAt(transactions.Count - 1);
        }

        if(requestModel.IsPrevious)
        {
            transactions.Reverse();
        }

        TransactionsSearchResponse response = new()
        {
            Draw = requestModel.Draw,
            Transactions = transactions,
            HasNext = hasNext,
        };

        return response;
    }

    public Task<List<Transaction>> GetUnevaluatedTransactions(int categoryid, int? lastId = null, DateTime? lastDate = null, int pageSize = 10)
    {
        return _unitOfWork.TransactionsRepository.GetUnevaluatedTransactions(categoryid, lastId, lastDate, pageSize);
    }
    public ValueTask<Transaction?> GetByIDAsync(int id)
    {
        return _unitOfWork.TransactionsRepository.GetByIDAsync(id);
    }

    public Transaction? GetByID(int id)
    {
        return _unitOfWork.TransactionsRepository.GetByID(id);
    }

    public async Task<Transaction> AddTransaction(TransactionPost transactionPost)
    {
        var transaction = new Transaction()
        {
            Title = transactionPost.Title,
            DateTime = transactionPost.DateTime,
            Amount = transactionPost.Amount,
            IsHappy = transactionPost.IsHappy,
            IsNecessary = transactionPost.IsNecessary,
            CategoryId = transactionPost.CategoryId,
        };

        _unitOfWork.TransactionsRepository.Insert(transaction);

        await _unitOfWork.Save();

        return transaction;
    }

    public async Task UpdateTransaction(Transaction transaction, TransactionPut transactionPut)
    {
        transaction.Title = transactionPut.Title;
        transaction.Description = transactionPut.Description;
        transaction.Amount = transactionPut.Amount;
        transaction.IsHappy = transactionPut.IsHappy;
        transaction.IsNecessary = transactionPut.IsNecessary;
        transaction.DateTime = transactionPut.DateTime;        
        transaction.Evaluated = transactionPut.Evaluated;
        transaction.PreviousIsNecessary = transactionPut.PreviousIsNecessary;
        transaction.PreviousIsHappy = transactionPut.PreviousIsHappy;

        await _unitOfWork.Save();
    }

    public async Task DeleteTransaction(Transaction transaction)
    {
        _unitOfWork.TransactionsRepository.Delete(transaction);
        await _unitOfWork.Save();
    }
}


