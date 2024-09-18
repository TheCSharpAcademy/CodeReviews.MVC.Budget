using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Budget.Domain.Entities;

namespace Budget.Infrastructure.Models;

/// <summary>
/// Represents a Transaction entity in the infrastructure layer.
/// </summary>
[Table("Transaction")]
internal class TransactionModel
{
    #region Constructors

    public TransactionModel()
    {

    }

    public TransactionModel(TransactionEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name ?? "";
        Date = entity.Date;
        Amount = entity.Amount;
        CategoryId = entity.Category!.Id;
    }

    #endregion
    #region Properties

    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [DataType(DataType.Date), Required]
    public DateTime Date { get; set; }

    [Column(TypeName = "decimal(18,2)"), DataType(DataType.Currency), Required]
    public decimal Amount { get; set; }

    [ForeignKey(nameof(Category))]
    public Guid CategoryId { get; set; }

    public CategoryModel? Category { get; set; }

    #endregion
    #region Methods

    public TransactionEntity MapToDomain()
    {
        return new TransactionEntity
        {
            Id = this.Id,
            Name = this.Name,
            Date = this.Date,
            Amount = this.Amount,
            Category = this.Category?.MapToDomain(),
        };
    }

    #endregion
}
