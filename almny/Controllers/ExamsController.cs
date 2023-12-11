using Microsoft.AspNetCore.Mvc;

namespace almny.Controllers
{
    public class ExamsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
}
