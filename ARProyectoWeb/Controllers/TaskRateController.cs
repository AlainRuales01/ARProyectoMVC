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
    public class TaskRateController : Controller
    {
        ARProyectoBO arProyectoBO = new ARProyectoBO();

        public IActionResult TaskRateTeacher(int courseId, int taskId)
        {
            var addTaskRate = new AddTaskRateViewModel();

            addTaskRate.Estudiantes = arProyectoBO.FindUsuarioTaskRates(courseId, taskId);
            addTaskRate.CourseId = courseId;
            addTaskRate.TaskId = taskId;

            return View(addTaskRate);
        }

        //[HttpPost]
        //public IActionResult TaskRateTeacher(AddTaskRateViewModel model)
        //{
        //    var taskCourse = _context.TaskCourse.Where(t => t.CourseId == model.CourseId && t.TaskId == model.TaskId).FirstOrDefault();

        //    TaskRate taskRate = _context.TaskRate.Where(t => t.TaskCourseId == taskCourse.TaskCourseId).FirstOrDefault();

        //    if (taskRate == null)
        //    {
        //        TaskRate rate = new TaskRate();
        //        rate.TaskCourseId = taskCourse.TaskCourseId;
        //        rate.UsuarioId = model.UsuarioId;
        //        rate.Calificacion = model.Calificacion;
        //        _context.TaskRate.Add(rate);
        //        _context.SaveChanges();
        //    }
        //    else
        //    {
        //        taskRate.Calificacion = model.Calificacion;
        //        _context.Entry(taskRate).State = EntityState.Modified;
        //        _context.SaveChanges();
        //    }

        //    return View();
        //}

        //public IActionResult TaskRateStudent(int courseID, int usuarioId)
        //{
        //    var addTaskRate = new AddTaskRateViewModel();
        //    var taskCourseId = _context.TaskCourse.Where(c => c.CourseId == courseID).Select(u => u.TaskId);

        //    ViewBag.TaskId = new SelectList(_context.Task.Where(u => taskCourseId.Contains(u.TaskId)), "TaskId", "Titulo");
        //    addTaskRate.CourseId = courseID;
        //    addTaskRate.UsuarioId = usuarioId;

        //    return View(addTaskRate);
        //}

        //[HttpPost]
        //public IActionResult TaskRateStudent(AddTaskRateViewModel model)
        //{
        //    var taskCourse = _context.TaskCourse.Where(t => t.CourseId == model.CourseId && t.TaskId == model.TaskId).FirstOrDefault();

        //    TaskRate taskRate = _context.TaskRate.Where(t => t.TaskCourseId == taskCourse.TaskCourseId).FirstOrDefault();

        //    if (taskRate == null)
        //    {
        //        TaskRate rate = new TaskRate();
        //        rate.TaskCourseId = taskCourse.TaskCourseId;
        //        rate.UsuarioId = model.UsuarioId;
        //        rate.CalificacionUsuario = model.Calificacion;
        //        _context.TaskRate.Add(rate);
        //        _context.SaveChanges();
        //    }
        //    else
        //    {
        //        taskRate.CalificacionUsuario = model.Calificacion;
        //        _context.Entry(taskRate).State = EntityState.Modified;
        //        _context.SaveChanges();
        //    }

        //    return View();
        //}
    }
}
