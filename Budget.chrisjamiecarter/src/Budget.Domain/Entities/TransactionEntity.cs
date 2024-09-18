namespace Budget.Domain.Entities;

/// <summary>
/// Represents a Transaction entity within the Domain layer.
/// </summary>
public class TransactionEntity
{
    #region Properties

    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public CategoryEntity? Category { get; set; }

    #endregion
}
