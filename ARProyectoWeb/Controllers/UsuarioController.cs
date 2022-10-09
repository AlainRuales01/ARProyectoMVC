using ARProyectoWeb.Data.Models;
using ARProyectoWeb.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ARProyectoWeb.Controllers
{
    [LoginFilter]
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

        [HttpPost]
        public IActionResult Create(Usuario nuevoUsuario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Error";
                return View(nuevoUsuario);
            }
            _context.Usuario.Add(nuevoUsuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? usuarioId)
        {
            var usuario = _context.Usuario.Find(usuarioId);
            if (usuario == null)
            {
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            var usuarioModificar = _context.Usuario.Find(usuario.UsuarioId);
            if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Correo))
            {
                ViewBag.Error = "Error";
                return View(usuario);
            }
            if (usuarioModificar != null)
            {
                usuarioModificar.Nombre = usuario.Nombre;
                usuarioModificar.Correo = usuario.Correo;
                _context.Entry(usuarioModificar).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? usuarioId)
        {
            var usuario = _context.Usuario.Find(usuarioId);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}