using ARProyectoWeb.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ARProyectoWeb.Controllers
{
    public class LoginController : Controller
    {

        private DataBaseContext _context;

        public LoginController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Usuario model)
        {

            if (string.IsNullOrEmpty(model.Correo) || string.IsNullOrEmpty(model.Clave))
            {
                ViewBag.Error = "Se debe insertar información en todos los campos";
                return View();
            }

            var usuario = _context.Usuario.Where(u => u.Correo == model.Correo && u.Clave == model.Clave).FirstOrDefault();

            if (usuario != null)
            {
                HttpContext.Session.SetString("UserName", usuario.Correo);
                HttpContext.Session.SetString("UserId", usuario.UsuarioId.ToString());
                HttpContext.Session.SetString("UserRol", usuario.Rol.ToString());
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "No se encontró el usuario especificado";
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.SetString("UserName", "");
            HttpContext.Session.SetString("UserId", "");
            return RedirectToAction("Index");
        }
    }


}
