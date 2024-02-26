using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TagHelpers_andApiTelegram.Models;

namespace TagHelpers_andApiTelegram.Controllers
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

        public string GetPerson(int id, string name, int age ) => $"Id: {id} Name: {name} Age: {age}";

        public IActionResult Privacy()
        {
            return View();
        }


    }
}
