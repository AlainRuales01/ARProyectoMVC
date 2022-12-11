using ARProyectoWeb.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace ARProyectoWeb.Controllers
{
    [LoginFilter]
    public class SelectionController : Controller
    {
        public IActionResult RecursoSelection()
        {
            return View();
        }

        public IActionResult EngagementSelection()
        {
            return View();
        }
    }
}
