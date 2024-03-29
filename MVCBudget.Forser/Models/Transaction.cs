﻿namespace MVCBudget.Forser.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter a transaction name")]
        [RegularExpression(@"^[a-zA-Z0-9\s.\-']{2,}$", ErrorMessage = "Name contains invalid characters.")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required, Precision(10, 2)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true), DisplayName("Transferred Amount")]
        public decimal TransferredAmount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), DisplayName("Transaction Date")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; } = null!;
        public int? UserWalletId { get; set; } = 0;
    }
}