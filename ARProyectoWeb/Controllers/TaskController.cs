using ARProyectoWeb.Data.Models;
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
            var rol = HttpContext.Session.GetString("UserRol");
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            List<Data.Models.Task> tasks = new List<Data.Models.Task>();
            if (rol == "Docente")
            {
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
            else if (rol == "Admin")
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
            return View(tasks);
        }

        public IActionResult Create()
        {
            ViewBag.UsuarioId = new SelectList(_context.Usuario.Where(u => u.Rol == "Docente"), "UsuarioId", "Correo");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Data.Models.Task nuevoTask)
        {
            if (string.IsNullOrEmpty(nuevoTask.Titulo) || string.IsNullOrEmpty(nuevoTask.Descripcion))
            {
                ViewBag.Error = "Se debe ingresar toda la información necesaria";
                return View(nuevoTask);
            }
            _context.Task.Add(nuevoTask);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? taskId)
        {
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
    }
}
