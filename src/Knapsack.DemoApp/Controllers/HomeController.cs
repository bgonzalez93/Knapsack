using Microsoft.AspNetCore.Mvc;

namespace Knapsack.DemoApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}