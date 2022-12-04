using ARProyectoWeb.Data.Models;
using ARProyectoWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ARProyectoWeb.Controllers
{
    public class TaskController : Controller
    {
        private DataBaseContext _context;

        public TaskController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            List<Data.Models.Task> tasks = new List<Data.Models.Task>();
            if (userRole == "Docente")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                tasks = (from t in _context.Task
                         join us in _context.Usuario on t.UsuarioId equals us.UsuarioId
                         where t.UsuarioId == userId
                         select new Data.Models.Task
                         {
                             TaskId = t.TaskId,
                             UsuarioId = t.UsuarioId,
                             Titulo = t.Titulo,
                             Descripcion = t.Descripcion,
                             UsuarioCreador = us
                         }).ToList();
            }
            else if (userRole == "Admin")
            {
                tasks = (from t in _context.Task
                         join us in _context.Usuario on t.UsuarioId equals us.UsuarioId
                         select new Data.Models.Task
                         {
                             TaskId = t.TaskId,
                             UsuarioId = t.UsuarioId,
                             Titulo = t.Titulo,
                             Descripcion = t.Descripcion,
                             UsuarioCreador = us
                         }).ToList();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(tasks);
        }

        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if(userRole == "Admin")
            {
                ViewBag.UsuarioId = new SelectList(_context.Usuario.Where(u => u.Rol == "Docente"), "UsuarioId", "Correo");
                
            }
            else if(userRole == "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();


        }

        [HttpPost]
        public IActionResult Create(Data.Models.Task nuevoTask)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(nuevoTask.Titulo) || string.IsNullOrEmpty(nuevoTask.Descripcion))
            {
                ViewBag.Error = "Se debe ingresar toda la información necesaria";
                return View(nuevoTask);
            }
            if(userRole == "Docente")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                nuevoTask.UsuarioId = userId;
            }
            _context.Task.Add(nuevoTask);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? taskId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if(userRole != "Admin")
            {
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioId = new SelectList(_context.Usuario.Where(u => u.Rol == "Docente"), "UsuarioId", "Correo");
            var curso = _context.Task.Find(taskId);
            if (curso == null)
            {
                return RedirectToAction("Index");
            }
            return View(curso);
        }

        [HttpPost]
        public IActionResult Edit(Data.Models.Task tarea)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index");
            }
            var tareaModificar = _context.Task.Find(tarea.TaskId);
            if (string.IsNullOrEmpty(tarea.Titulo) || string.IsNullOrEmpty(tarea.Descripcion))
            {
                ViewBag.Error = "Se debe ingresar toda la información necesaria";
                return View(tarea);
            }
            if (tareaModificar != null)
            {
                tareaModificar.Titulo = tarea.Titulo;
                tareaModificar.Descripcion = tarea.Descripcion;
                tareaModificar.UsuarioId = tarea.UsuarioId;
                _context.Entry(tareaModificar).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult AddTaskCourse(int taskId)
        {
            var taskCourseModel = new AddTaskCourseViewModel();
            var userRole = HttpContext.Session.GetString("UserRole");
            taskCourseModel.TaskId = taskId;
            if (userRole == "Docente")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                List<int> usuarioCourses = _context.UsuarioCourse.Where(u => u.UsuarioId == userId).Select(c => c.CourseId).Distinct().ToList();
                ViewBag.CourseId = new SelectList(_context.Course.Where(c => usuarioCourses.Contains(c.CourseId)), "CourseId", "Nombre");
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
            /*Validar que no se agregue dos veces un usuario al mismo curso*/
            var taskCourse = new TaskCourse();
            taskCourse.TaskId = model.TaskId;
            taskCourse.CourseId = model.CourseId;
            taskCourse.CalificacionProfesor = model.Calificacion;
            _context.TaskCourse.Add(taskCourse);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
