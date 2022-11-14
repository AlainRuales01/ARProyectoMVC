using ARProyectoWeb.Data.Models;
using ARProyectoWeb.Utilities;
using ARProyectoWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

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
            List<Usuario> usuarios = _context.Usuario.Where(u => u.Rol != "Admin").ToList();
            return View(usuarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario nuevoUsuario)
        {
            if (string.IsNullOrEmpty(nuevoUsuario.Nombres) || string.IsNullOrEmpty(nuevoUsuario.Apellidos) || string.IsNullOrEmpty(nuevoUsuario.Correo) || string.IsNullOrEmpty(nuevoUsuario.Clave) || nuevoUsuario.FechaNacimiento == default(DateTime))
            {
                ViewBag.Error = "Ingrese toda la información necesaria";
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
            if (string.IsNullOrEmpty(usuario.Nombres) || string.IsNullOrEmpty(usuario.Apellidos) || string.IsNullOrEmpty(usuario.Correo))
            {
                ViewBag.Error = "Ingrese toda la información necesaria";
                return View(usuario);
            }
            if (usuarioModificar != null)
            {
                
                usuarioModificar.Nombres = usuario.Nombres;
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

        public IActionResult AddUserCourse(int usuarioId)
        {
            ViewBag.CourseId = new SelectList(_context.Course, "CourseId", "Nombre");
            var usuarioCourseModel = new AddUserCourseViewModel();
            usuarioCourseModel.UsuarioId = usuarioId;
            return View(usuarioCourseModel);
        }

        [HttpPost]
        public IActionResult AddUserCourse(AddUserCourseViewModel model)
        {
            /*Validar que no se agregue dos veces un usuario al mismo curso*/
            var usuarioCourse = new UsuarioCourse();
            usuarioCourse.CourseId = model.CourseId;
            usuarioCourse.UsuarioId = model.UsuarioId;
            _context.UsuarioCourse.Add(usuarioCourse);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}