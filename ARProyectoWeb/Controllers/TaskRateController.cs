using ARProyectoWeb.Data.Models;
using ARProyectoWeb.Utilities;
using ARProyectoWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ARProyectoWeb.Controllers
{
    [LoginFilter]
    public class TaskRateController : Controller
    {
        private DataBaseContext _context;

        public TaskRateController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult TaskRateTeacher(int courseId, int taskId)
        {
            var addTaskRate = new AddTaskRateViewModel();
            var taskCourse = _context.TaskCourse.Where(t => t.CourseId == courseId && t.TaskId == taskId).FirstOrDefault();
            var usuariosCourseId = _context.UsuarioCourse.Where(c => c.CourseId == courseId).Select(u => u.UsuarioId).ToList();
            addTaskRate.Estudiantes = (from e in _context.Usuario
                                       where usuariosCourseId.Contains(e.UsuarioId) && e.Rol != "Doncente"
                                       select new UsuarioModel
                                       {
                                           UsuarioId = e.UsuarioId,
                                           Nombres = e.Nombres,
                                           Apellidos = e.Apellidos,
                                           Correo = e.Correo,
                                           FechaNacimiento = e.FechaNacimiento
                                       }
                                       ).ToList();
            addTaskRate.CourseId = courseId;
            addTaskRate.CalificacionesEstudiantes = _context.TaskRate.Where(t => usuariosCourseId.Contains(t.UsuarioId) && t.TaskCourseId == taskCourse.TaskCourseId).ToList();
            addTaskRate.TaskId = taskId;

            return View(addTaskRate);
        }

        [HttpPost]
        public IActionResult TaskRateTeacher(AddTaskRateViewModel model)
        {
            var taskCourse = _context.TaskCourse.Where(t => t.CourseId == model.CourseId && t.TaskId == model.TaskId).FirstOrDefault();

            TaskRate taskRate = _context.TaskRate.Where(t => t.TaskCourseId == taskCourse.TaskCourseId).FirstOrDefault();

            if(taskRate == null)
            {
                TaskRate rate = new TaskRate();
                rate.TaskCourseId = taskCourse.TaskCourseId;
                rate.UsuarioId = model.UsuarioId;
                rate.Calificacion = model.Calificacion;
                _context.TaskRate.Add(rate);
                _context.SaveChanges();
            }
            else
            {
                taskRate.Calificacion = model.Calificacion;
                _context.Entry(taskRate).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return View();
        }

        public IActionResult TaskRateStudent(int courseID, int usuarioId)
        {
            var addTaskRate = new AddTaskRateViewModel();
            var taskCourseId = _context.TaskCourse.Where(c => c.CourseId == courseID).Select(u => u.TaskId);

            ViewBag.TaskId = new SelectList(_context.Task.Where(u => taskCourseId.Contains(u.TaskId)), "TaskId", "Titulo");
            addTaskRate.CourseId = courseID;
            addTaskRate.UsuarioId= usuarioId;

            return View(addTaskRate);
        }

        [HttpPost]
        public IActionResult TaskRateStudent(AddTaskRateViewModel model)
        {
            var taskCourse = _context.TaskCourse.Where(t => t.CourseId == model.CourseId && t.TaskId == model.TaskId).FirstOrDefault();

            TaskRate taskRate = _context.TaskRate.Where(t => t.TaskCourseId == taskCourse.TaskCourseId).FirstOrDefault();

            if (taskRate == null)
            {
                TaskRate rate = new TaskRate();
                rate.TaskCourseId = taskCourse.TaskCourseId;
                rate.UsuarioId = model.UsuarioId;
                rate.CalificacionUsuario = model.Calificacion;
                _context.TaskRate.Add(rate);
                _context.SaveChanges();
            }
            else
            {
                taskRate.CalificacionUsuario = model.Calificacion;
                _context.Entry(taskRate).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return View();
        }
    }
}
