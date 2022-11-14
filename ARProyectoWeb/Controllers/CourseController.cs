using ARProyectoWeb.Data.Models;
using ARProyectoWeb.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ARProyectoWeb.Controllers
{
    [LoginFilter]
    public class CourseController : Controller
    {
        private DataBaseContext _context;

        public CourseController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("UserRol");
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            List<Course> cursos = new List<Course>();
            ;
            if (rol == "Docente")
            {
                List<int> usuarioCourses = _context.UsuarioCourse.Where(u => u.UsuarioId == userId).Select(c => c.CourseId).ToList();
                cursos = _context.Course.Where(c => usuarioCourses.Contains(c.CourseId)).ToList();
            }
            else if (rol == "Admin")
            {
                cursos = _context.Course.ToList();
            }
            return View(cursos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Course nuevoCurso)
        {
            if (string.IsNullOrEmpty(nuevoCurso.Nombre) || string.IsNullOrEmpty(nuevoCurso.Descripcion)  || nuevoCurso.FechaInicio == default(DateTime) || nuevoCurso.FechaFin == default(DateTime))
            {
                ViewBag.Error = "Se debe ingresar toda la información necesaria";
                return View(nuevoCurso);
            }
            _context.Course.Add(nuevoCurso);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? courseId)
        {
            var curso = _context.Course.Find(courseId);
            if (curso == null)
            {
                return RedirectToAction("Index");
            }
            return View(curso);
        }

        [HttpPost]
        public IActionResult Edit(Course curso)
        {
            var cursoModificar = _context.Course.Find(curso.CourseId);
            if (string.IsNullOrEmpty(curso.Nombre) || string.IsNullOrEmpty(curso.Descripcion))
            {
                ViewBag.Error = "Se debe ingresar toda la información necesaria";
                return View(curso);
            }
            if (cursoModificar != null)
            {

                cursoModificar.Nombre = curso.Nombre;
                cursoModificar.Descripcion = curso.Descripcion;
                _context.Entry(cursoModificar).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ListaUsuariosCourse(int courseId) {
            ViewBag.CourseId = new SelectList(_context.Course, "CourseId", "Nombre");
            List<Usuario> usuarios = new List<Usuario>();
            if(courseId != 0)
            {
                var usuariosCourseId = _context.UsuarioCourse.Where(c => c.CourseId == courseId).Select(c => c.UsuarioId).ToList();
                usuarios = _context.Usuario.Where(u => usuariosCourseId.Contains(u.UsuarioId)).ToList();
            }
            return View(usuarios);
        }


    }
}