using Microsoft.AspNetCore.Mvc;
using MVC.Budget.JsPeanut.Models;
using System.Diagnostics;

namespace MVC.Budget.JsPeanut.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}