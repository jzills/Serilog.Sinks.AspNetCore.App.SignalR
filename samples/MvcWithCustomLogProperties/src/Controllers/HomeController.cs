using Microsoft.AspNetCore.Mvc;

namespace MvcWithCustomLogProperties.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
