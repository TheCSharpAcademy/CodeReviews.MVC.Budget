using System;
using System.Collections.Generic;

namespace Budget.Models;

public class IndexViewModel
{
    public List<Category> Categories {get; set;} = [];
    public List<Transaction> Transactions {get; set;} = [];

    public string? TransactionName { get; set; }
    public string? CategoryName { get; set; }
    public DateOnly? TransactionDate {get; set;}
}