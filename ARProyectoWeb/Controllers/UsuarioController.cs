using ARProyectoWeb.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            var usuarioModificar = _context.Usuario.Find(usuario.UsuarioId);
            if(usuarioModificar != null)
            {
                usuarioModificar.Nombre = usuario.Nombre;
                usuarioModificar.Correo = usuario.Correo;
                _context.Entry(usuarioModificar).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        public IActionResult Delete(int? usuarioId)
        {
            var usuario = _context.Usuario.Find(usuarioId);
            return View(usuario);
        }
    }
}