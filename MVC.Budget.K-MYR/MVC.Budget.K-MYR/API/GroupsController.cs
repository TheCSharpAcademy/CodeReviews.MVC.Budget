using Microsoft.AspNetCore.Mvc;
using MVC.Budget.K_MYR.Data;

namespace MVC.Budget.K_MYR.API;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoriesController> _logger;

    public GroupsController(IUnitOfWork unitOfWork, ILogger<CategoriesController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
}