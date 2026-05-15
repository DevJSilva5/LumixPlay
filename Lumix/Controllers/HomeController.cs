using Microsoft.AspNetCore.Mvc;

namespace Lumix.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}