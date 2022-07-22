using Microsoft.AspNetCore.Mvc;

namespace Quizard.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
