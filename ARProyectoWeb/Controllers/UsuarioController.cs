using ARProyectoWeb.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ARProyectoWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private DataBaseContext _context;

        public UsuarioController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Usuario> usuarios = _context.Usuario.ToList();
            return View(usuarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int? usuarioId)
        {
            var usuario = _context.Usuario.Find(usuarioId);
            return View(usuario);
        }

        public IActionResult Delete(int? usuarioId)
        {
            var usuario = _context.Usuario.Find(usuarioId);
            return View(usuario);
        }
    }
}
