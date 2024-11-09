namespace MVC.Budget.K_MYR.Models;

public class TransactionsSearchResponse
{
    public int? Draw { get; set; }
    public bool HasNext { get; set; }
    public List<TransactionDTO> Transactions { get; set; } = new List<TransactionDTO>();
}
