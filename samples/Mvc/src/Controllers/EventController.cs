using Microsoft.AspNetCore.Mvc;

namespace Mvc.Controllers;

public class EventController : Controller
{
    [HttpGet]
    public IActionResult Get() => Ok();

    [HttpPost]
    public IActionResult Post() => Ok();

    [HttpPut]
    public IActionResult Put() => Ok();

    [HttpDelete]
    public IActionResult Delete() => Ok();
}
