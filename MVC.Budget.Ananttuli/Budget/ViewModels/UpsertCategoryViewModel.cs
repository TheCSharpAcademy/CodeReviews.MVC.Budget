using Budget.CategoriesModule.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.ViewModels;

public class UpsertCategoryViewModel
{
    public Category? Category { get; set; }
}