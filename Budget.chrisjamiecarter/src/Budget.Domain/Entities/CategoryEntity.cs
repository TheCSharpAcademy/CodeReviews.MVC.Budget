namespace Budget.Domain.Entities;

/// <summary>
/// Represents a Category entity within the Domain layer.
/// </summary>
public class CategoryEntity
{
    #region Properties

    public Guid Id { get; set; }

    public string? Name { get; set; }

    #endregion
}
