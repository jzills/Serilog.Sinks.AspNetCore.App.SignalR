using Microsoft.AspNetCore.Mvc;

namespace MvcLoggerConfiguration.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
