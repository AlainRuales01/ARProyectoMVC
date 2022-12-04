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
            List<Usuario> usuarios = new List<Usuario>();
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin")
            {
                usuarios = _context.Usuario.Where(u => u.Rol != "Admin").ToList();
            }else if (userRole == "Docente")
            {
                usuarios = _context.Usuario.Where(u => u.Rol != "Admin" && u.Rol != "Docente").ToList();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(usuarios);
        }

        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin" || userRole == "Docente")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpPost]
        public IActionResult Create(Usuario nuevoUsuario)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Estudiante")
            {
                return RedirectToAction("Index");
            }

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
            var userRole = HttpContext.Session.GetString("UserRole");
            if (usuario == null)
            {
                return RedirectToAction("Index");
            }
            if (userRole == "Admin" || userRole == "Docente")
            {
                return View(usuario);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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

        //public IActionResult Delete(int? usuarioId)
        //{
        //    var usuario = _context.Usuario.Find(usuarioId);
        //    if (usuario != null)
        //    {
        //        _context.Usuario.Remove(usuario);
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        public IActionResult AddUserCourse(int usuarioId)
        {
            var usuarioCourseModel = new AddUserCourseViewModel();
            var userRole = HttpContext.Session.GetString("UserRole");
            usuarioCourseModel.UsuarioId = usuarioId;
            if (userRole == "Admin")
            {
                ViewBag.CourseId = new SelectList(_context.Course, "CourseId", "Nombre");
            }
            else if (userRole == "Docente")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                List<int> usuarioCourses = _context.UsuarioCourse.Where(u => u.UsuarioId == userId).Select(c => c.CourseId).Distinct().ToList();
                ViewBag.CourseId = new SelectList(_context.Course.Where(c => usuarioCourses.Contains(c.CourseId)), "CourseId", "Nombre");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(usuarioCourseModel);
        }

        [HttpPost]
        public IActionResult AddUserCourse(AddUserCourseViewModel model)
        {
            /*Validar que no se agregue dos veces un usuario al mismo curso*/
            var usuarioCourse = new UsuarioCourse();
            usuarioCourse.UsuarioId = model.UsuarioId;
            usuarioCourse.CourseId = model.CourseId;
            _context.UsuarioCourse.Add(usuarioCourse);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}