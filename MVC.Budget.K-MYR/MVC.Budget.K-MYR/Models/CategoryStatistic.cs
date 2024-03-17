using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.K_MYR.Models;

public class CategoryStatistic
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    [DataType(DataType.Currency)]
    [Precision(19, 4)]
    public decimal Budget { get; set; }
    public DateTime Month { get; set; }
    [DataType(DataType.Currency)]
    [Precision(19, 4)]
    public decimal TotalSpent { get; set; } = 0;
    [DataType(DataType.Currency)]
    [Precision(19, 4)]
    public decimal Overspending { get; set; } = 0;
    public int TotalTransactions { get; set; } = 0;
    public int HappyTransactions { get; set; } = 0;
    public int NecessaryTransactions { get; set; } = 0;
    public int HappyEvaluatedTransactions { get; set; } = 0;
    public int NecessaryEvaluatedTransactions { get; set; } = 0;

    public void AddTransaction(Transaction transaction)
    {
        TotalSpent += transaction.Amount;
        Overspending = Math.Max(0, TotalSpent - Budget);
        TotalTransactions++;

        if(transaction.Evaluated)
        {
            if (transaction.IsHappy)
                HappyEvaluatedTransactions++;

            if (transaction.IsNecessary)
                NecessaryEvaluatedTransactions++;

            if (transaction.PreviousIsHappy)
                HappyEvaluatedTransactions++;

            if (transaction.PreviousIsNecessary)
                NecessaryEvaluatedTransactions++;
        }

        else
        {
            if (transaction.IsHappy)
                HappyTransactions++;

            if (transaction.IsNecessary)
                NecessaryTransactions++;
        }
    }

    public void AddTransaction(TransactionPut transaction)
    {
        TotalSpent += transaction.Amount;
        Overspending = Math.Max(0, TotalSpent - Budget);
        TotalTransactions++;

        if (transaction.Evaluated)
        {
            if (transaction.IsHappy)
                HappyEvaluatedTransactions++;

            if (transaction.IsNecessary)
                NecessaryEvaluatedTransactions++;

            if (transaction.PreviousIsHappy)
                HappyEvaluatedTransactions++;

            if (transaction.PreviousIsNecessary)
                NecessaryEvaluatedTransactions++;
        }

        else
        {
            if (transaction.IsHappy)
                HappyTransactions++;

            if (transaction.IsNecessary)
                NecessaryTransactions++;
        }
    }

    public void RemoveTransaction(Transaction transaction)
    {
        TotalSpent -= transaction.Amount;
        Overspending = Math.Max(0, TotalSpent - Budget);
        TotalTransactions--;

        if (transaction.Evaluated)
        {
            if (transaction.IsHappy)
                HappyEvaluatedTransactions--;

            if (transaction.IsNecessary)
                NecessaryEvaluatedTransactions--;

            if (transaction.PreviousIsHappy)
                HappyEvaluatedTransactions--;

            if (transaction.PreviousIsNecessary)
                NecessaryEvaluatedTransactions--;
        }

        else
        {
            if (transaction.IsHappy)
                HappyTransactions--;

            if (transaction.IsNecessary)
                NecessaryTransactions--;
        }

    }
}
