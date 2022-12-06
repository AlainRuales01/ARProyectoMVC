using ARProyectoWeb.Business.BO;
using ARProyectoWeb.Business.Models;
using ARProyectoWeb.Data.Models;
using ARProyectoWeb.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace ARProyectoWeb.Controllers
{
    [LoginFilter]
    public class UsuarioController : Controller
    {
        ARProyectoBO arProyectoBO = new ARProyectoBO();

        public IActionResult Index()
        {
            List<Usuario> usuarios = new List<Usuario>();
            var userRole = HttpContext.Session.GetString("UserRole");
            
            arProyectoBO.FindUsuariosList(userRole);

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
            arProyectoBO.AddNewUsuario(nuevoUsuario);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int UsuarioId)
        {
            var usuario = arProyectoBO.FindUsuarioById(UsuarioId);
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
            if (string.IsNullOrEmpty(usuario.Nombres) || string.IsNullOrEmpty(usuario.Apellidos) || string.IsNullOrEmpty(usuario.Correo))
            {
                ViewBag.Error = "Ingrese toda la información necesaria";
                return View(usuario);
            }
            arProyectoBO.EditUsuario(usuario);
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
            var cursos = new List<Course>();

            usuarioCourseModel.UsuarioId = usuarioId;
            
            if (userRole == "Admin")
            {
                cursos = arProyectoBO.FindCourses();
            }
            else if (userRole == "Docente")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                cursos = arProyectoBO.FindUsuarioCourses(userId);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CourseId = new SelectList(cursos, "CourseId", "Nombre");
            return View(usuarioCourseModel);
        }

        [HttpPost]
        public IActionResult AddUserCourse(AddUserCourseViewModel model)
        {
            /*Validar que no se agregue dos veces un usuario al mismo curso*/
            var usuarioCourse = new UsuarioCourse();

            usuarioCourse.UsuarioId = model.UsuarioId;
            usuarioCourse.CourseId = model.CourseId;

            arProyectoBO.AddUsuarioCourse(usuarioCourse);
            return RedirectToAction("Index");
        }
    }
}