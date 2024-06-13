using Microsoft.AspNetCore.Mvc;

namespace MvcWithDefaultHub.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
