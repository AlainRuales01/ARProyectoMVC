using ARProyectoWeb.Business.BO;
using ARProyectoWeb.Business.Models;
using ARProyectoWeb.Data.Models;
using ARProyectoWeb.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ARProyectoWeb.Controllers
{
    [LoginFilter]
    public class TaskController : Controller
    {

        ARProyectoBO arProyectoBO = new ARProyectoBO();

        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }

            List<Data.Models.Task> tasks = new List<Data.Models.Task>();
            if (userRole == "Docente")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                tasks = arProyectoBO.FindUsuarioTasks(userId);
            }
            else if (userRole == "Admin")
            {
                tasks = arProyectoBO.FindTasks();
            }
            return View(tasks);
        }

        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }

            if (userRole == "Admin")
            {
                var docentes = arProyectoBO.FindDocentes();
                ViewBag.UsuarioId = new SelectList(docentes, "UsuarioId", "Correo");
            }

            return View();


        }

        [HttpPost]
        public IActionResult Create(Data.Models.Task nuevoTask)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(nuevoTask.Titulo) || string.IsNullOrEmpty(nuevoTask.Descripcion))
            {
                ViewBag.Error = "Se debe ingresar toda la información necesaria";
                return View(nuevoTask);
            }

            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            arProyectoBO.AddNewTask(nuevoTask, userId);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int taskId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var docentes = arProyectoBO.FindDocentes();

            ViewBag.UsuarioId = new SelectList(docentes, "UsuarioId", "Correo");

            var task = arProyectoBO.FindTaskById(taskId);
            if (task == null)
            {
                return RedirectToAction("Index");
            }
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(Data.Models.Task task)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(task.Titulo) || string.IsNullOrEmpty(task.Descripcion))
            {
                ViewBag.Error = "Se debe ingresar toda la información necesaria";
                return View(task);
            }

            arProyectoBO.EditTask(task);

            return RedirectToAction("Index");
        }

        public IActionResult AddTaskCourse(int taskId)
        {
            var taskCourseModel = new AddTaskCourseViewModel();
            var userRole = HttpContext.Session.GetString("UserRole");
            var cursos = new List<Course>();
            taskCourseModel.TaskId = taskId;
            if (userRole == "Docente")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                cursos = arProyectoBO.FindUsuarioCourses(userId);
                ViewBag.CourseId = new SelectList(cursos, "CourseId", "Nombre");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(taskCourseModel);
        }

        [HttpPost]
        public IActionResult AddTaskCourse(AddTaskCourseViewModel model)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Docente")
            {
                return RedirectToAction("Index", "Home");
            }

            if(model.Calificacion > 0 && model.Calificacion <= 10)
            {
                var taskCourse = new TaskCourse();
                taskCourse.TaskId = model.TaskId;
                taskCourse.CourseId = model.CourseId;
                taskCourse.CalificacionProfesor = model.Calificacion;

                arProyectoBO.AddTaskCourse(taskCourse);

                return RedirectToAction("Index");
            }
            else
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                var cursos = arProyectoBO.FindUsuarioCourses(userId);
                ViewBag.CourseId = new SelectList(cursos, "CourseId", "Nombre");
                ViewBag.Error = "Ingrese un número entre 1 y 10";
                return View(model);
            }
            
        }
    }
}
