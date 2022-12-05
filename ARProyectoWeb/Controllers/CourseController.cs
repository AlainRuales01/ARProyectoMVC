using ARProyectoWeb.Business.BO;
using ARProyectoWeb.Data.Models;
using ARProyectoWeb.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ARProyectoWeb.Controllers
{
    [LoginFilter]
    public class CourseController : Controller
    {
        private DataBaseContext _context;
        ARProyectoBO arProyectoBO = new ARProyectoBO();

        public CourseController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            List<Course> cursos = new List<Course>();
            if (userRole == "Docente" || userRole == "Estudiante")
            {
                List<int> usuarioCourses = _context.UsuarioCourse.Where(u => u.UsuarioId == userId).Select(c => c.CourseId).Distinct().ToList();
                cursos = _context.Course.Where(c => usuarioCourses.Contains(c.CourseId)).ToList();
            }
            else if (userRole == "Admin")
            {
                cursos = _context.Course.ToList();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(cursos);
        }

        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public IActionResult Create(Course nuevoCurso)
        {
            if (string.IsNullOrEmpty(nuevoCurso.Nombre) || string.IsNullOrEmpty(nuevoCurso.Descripcion) || nuevoCurso.FechaInicio == default(DateTime) || nuevoCurso.FechaFin == default(DateTime))
            {
                ViewBag.Error = "Se debe ingresar toda la información necesaria";
                return View(nuevoCurso);
            }
            
            arProyectoBO.AddNewCourse(nuevoCurso);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int CourseId)
        {
            var curso = arProyectoBO.FindCourseById(CourseId);
            if (curso == null)
            {
                return RedirectToAction("Index");
            }
            return View(curso);
        }

        [HttpPost]
        public IActionResult Edit(Course curso)
        {
            var cursoModificar = arProyectoBO.FindCourseById(curso.CourseId);
            if (string.IsNullOrEmpty(curso.Nombre) || string.IsNullOrEmpty(curso.Descripcion))
            {
                ViewBag.Error = "Se debe ingresar toda la información necesaria";
                return View(curso);
            }
            if (cursoModificar != null)
            {

                cursoModificar.Nombre = curso.Nombre;
                cursoModificar.Descripcion = curso.Descripcion;
                arProyectoBO.EditCourse(cursoModificar);
            }
            return RedirectToAction("Index");
        }

        public IActionResult ListaUsuariosCourse(int courseId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin")
            {
                ViewBag.CourseId = new SelectList(_context.Course, "CourseId", "Nombre");
            }
            else if (userRole == "Docente" || userRole == "Estudiante")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                var usuariosCourseId = _context.UsuarioCourse.Where(c => c.UsuarioId == userId).Select(c => c.CourseId).ToList();
                ViewBag.CourseId = new SelectList(_context.Course.Where(c => usuariosCourseId.Contains(c.CourseId)), "CourseId", "Nombre");
            }

            List<Usuario> usuarios = new List<Usuario>();
            if (courseId != 0)
            { 
                var usuariosCourseId = _context.UsuarioCourse.Where(c => c.CourseId == courseId).Select(c => c.UsuarioId).ToList();
                usuarios = _context.Usuario.Where(u => usuariosCourseId.Contains(u.UsuarioId)).ToList();
            }
            return View(usuarios);
        }

        public IActionResult ListaTaskCourse(int courseId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin")
            {
                ViewBag.CourseId = new SelectList(_context.Course, "CourseId", "Nombre");
            }
            else if (userRole == "Docente" || userRole == "Estudiante")
            {
                ViewBag.courseSelectedId = courseId;
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                var usuariosCourseId = _context.UsuarioCourse.Where(c => c.UsuarioId == userId).Select(c => c.CourseId).ToList();
                ViewBag.CourseId = new SelectList(_context.Course.Where(c => usuariosCourseId.Contains(c.CourseId)), "CourseId", "Nombre");
            }

            List<Data.Models.Task> tasks = new List<Data.Models.Task>();
            if (courseId != 0)
            {
                var taskCourseId = _context.TaskCourse.Where(c => c.CourseId == courseId).Select(c => c.TaskId).ToList();
                tasks = _context.Task.Where(u => taskCourseId.Contains(u.TaskId)).ToList();
            }
            return View(tasks);
        }




    }
}