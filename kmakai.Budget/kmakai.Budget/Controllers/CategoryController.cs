using kmakai.Budget.Context;
using kmakai.Budget.Models;
using kmakai.Budget.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace kmakai.Budget.Controllers;

public class CategoryController : Controller
{
    private readonly IRepository _repository;

    public CategoryController(IRepository repository)
    {
        _repository = repository;
    }

    public List<Category> GetCategories()
    {
        return _repository.GetCategories();
    }

    [HttpPost]
    public ActionResult AddCategory(Category category)
    {
        _repository.AddCategory(category);
        return Ok();
    }

    //[HttpDelete]
    //public ActionResult DeleteCategory(int id)
    //{
    //    _repository.DeleteCategory(id);
    //    return Ok();
    //}

    [HttpPut]
    public ActionResult UpdateCategory(Category category)
    {
        _repository.UpdateCategory(category);
        return Ok();
    }

    [HttpPost]
    public ActionResult DeleteCategory(int id)
    {
        _repository.DeleteCategory(id);
        return Ok();
    }

}
