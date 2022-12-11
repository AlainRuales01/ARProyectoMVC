using ARProyectoWeb.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace ARProyectoWeb.Controllers
{
    [LoginFilter]
    public class SelectionController : Controller
    {
        public IActionResult RecursoSelection()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult EngagementSelection()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
