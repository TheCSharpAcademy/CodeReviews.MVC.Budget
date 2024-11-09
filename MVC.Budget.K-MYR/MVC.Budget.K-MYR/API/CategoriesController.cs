using Microsoft.AspNetCore.Mvc;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Services;

namespace MVC.Budget.K_MYR.API;

[Route("api/[controller]")]
public class ExpenseCategoriesController(ILogger<GenericCategoriesController<ExpenseCategory>> logger,
                                         ICategoriesService categoriesService) :
    GenericCategoriesController<ExpenseCategory>(logger, categoriesService)
{
}

[Route("api/[controller]")]
public class IncomeCategoriesController(ILogger<GenericCategoriesController<IncomeCategory>> logger,
                                        ICategoriesService categoriesService) :
    GenericCategoriesController<IncomeCategory>(logger, categoriesService)
{
}
