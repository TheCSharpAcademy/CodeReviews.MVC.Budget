using Budget.Doc415.Models;
using System.Globalization;

namespace Budget.Doc415.Transformations;

static public class Transformer
{
    static public DateTime ConvertToDateTime(string date)
    {
        try
        {
            var isValid = DateTime.TryParseExact(date, ["yyyy-MM-dd", "dd-MM-yyyy"], System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime transActionDate);
            return transActionDate;
        }
        catch
        {
            Console.Error.WriteLine("Not a valid datetime");
            return DateTime.MinValue;
        }
    }

    static public List<TransactionViewModel> ConvertToTransactionVM(List<Transaction> transactions, List<Category> categories)
    {
        List<TransactionViewModel> viewModelList = new List<TransactionViewModel>();
        foreach (var transaction in transactions)
        {
            var categoryName = categories.Single(x => x.Id == transaction.RefCatId).Name;
            TransactionViewModel newVM = new()
            {
                Id = transaction.Id,
                Name = transaction.Name,
                Category = categoryName,
                Date = transaction.Date,
                Description = transaction.Description,
                Amount = transaction.Amount
            };
            viewModelList.Add(newVM);
        }
        return viewModelList;
    }

    static public Transaction ConvertToDbTransaction(TransactionViewModel transactionVM, List<Category> categories)
    {
        var transaction = new Transaction();
        var categoryId = categories.FirstOrDefault(x => x.Name == transactionVM.Category).Id;
        transaction.Id = transactionVM.Id;
        transaction.Name = transactionVM.Name;
        transaction.Description = transactionVM.Description;
        transaction.Amount = transactionVM.Amount;
        transaction.Date = transactionVM.Date;
        transaction.RefCatId = categoryId;
        return transaction;
    }
}
