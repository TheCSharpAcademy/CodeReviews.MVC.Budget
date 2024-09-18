using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Budget.Domain.Entities;

namespace Budget.Infrastructure.Models;

/// <summary>
/// Represents a Category entity in the infrastructure layer.
/// </summary>
[Table("Category")]
internal class CategoryModel
{
    #region Constructors

    public CategoryModel()
    {

    }

    public CategoryModel(CategoryEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name ?? "";
        Transactions = [];
    }

    #endregion
    #region Properties

    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public List<TransactionModel>? Transactions { get; set; }

    #endregion
    #region Methods

    public CategoryEntity MapToDomain()
    {
        return new CategoryEntity
        {
            Id = this.Id,
            Name = this.Name,
        };
    }

    #endregion
}
