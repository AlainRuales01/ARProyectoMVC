using ARProyectoWeb.Business.BO;
using ARProyectoWeb.Data.Models;
using ARProyectoWeb.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ARProyectoWeb.Controllers
{
    [LoginFilter]
    public class CourseController : Controller
    {
        ARProyectoBO arProyectoBO = new ARProyectoBO();

        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }
            List<Course> cursos = new List<Course>();
            if (userRole == "Docente")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                cursos = arProyectoBO.FindUsuarioCourses(userId);
            }
            else if (userRole == "Admin")
            {
                cursos = arProyectoBO.FindCourses();
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
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

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
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }

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
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(curso.Nombre) || string.IsNullOrEmpty(curso.Descripcion))
            {
                ViewBag.Error = "Se debe ingresar toda la información necesaria";
                return View(curso);
            }
            arProyectoBO.EditCourse(curso);
            return RedirectToAction("Index");
        }

        public IActionResult ListaUsuariosCourse(int courseId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }

            var cursos = new List<Course>();
            if (userRole == "Admin")
            {
                cursos = arProyectoBO.FindCourses();
            }
            else if (userRole == "Docente")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                cursos = arProyectoBO.FindUsuarioCourses(userId);

            }

            ViewBag.CourseId = new SelectList(cursos, "CourseId", "Nombre");
            List<Usuario> usuarios = new List<Usuario>();

            if (courseId != 0)
            {
                usuarios = arProyectoBO.FindCourseUsuarios(courseId);
            }
            return View(usuarios);
        }

        public IActionResult ListaTaskCourse(int courseId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var cursos = new List<Course>();
            ViewBag.courseSelectedId = courseId;
            if (userRole == "Admin")
            {
                cursos = arProyectoBO.FindCourses();
            }
            else if (userRole == "Docente" || userRole == "Estudiante")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                cursos = arProyectoBO.FindUsuarioCourses(userId);
            }

            ViewBag.CourseId = new SelectList(cursos, "CourseId", "Nombre");
            List<Data.Models.Task> tasks = new List<Data.Models.Task>();

            if (courseId != 0)
            {
                if (userRole == "Estudiante")
                {
                    return RedirectToAction("TaskRateStudent", "TaskRate", new {courseId = courseId});
                }

                tasks = arProyectoBO.FindCourseTasks(courseId);
            }
            return View(tasks);
        }
    }
}