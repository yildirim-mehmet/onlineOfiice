using Microsoft.AspNetCore.Mvc;

namespace OfficeIMO.Collaborative.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
