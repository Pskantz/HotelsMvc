using Microsoft.AspNetCore.Mvc;

namespace HotelsApp.Controllers;

public class HotelsAppController() : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}