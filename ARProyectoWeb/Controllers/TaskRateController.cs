using ARProyectoWeb.Business.BO;
using ARProyectoWeb.Business.Models;
using ARProyectoWeb.Data.Models;
using ARProyectoWeb.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ARProyectoWeb.Controllers
{
    [LoginFilter]
    public class TaskRateController : Controller
    {
        ARProyectoBO arProyectoBO = new ARProyectoBO();

        public IActionResult TaskRateTeacher(int courseId, int taskId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Docente")
            {
                return RedirectToAction("Index", "Home");
            }
            var addTaskRate = new AddTaskRateViewModel();

            addTaskRate.Estudiantes = arProyectoBO.FindUsuarioTaskRates(courseId, taskId);
            addTaskRate.CourseId = courseId;
            addTaskRate.TaskId = taskId;

            return View(addTaskRate);
        }

        [HttpPost]
        public IActionResult TaskRateTeacher(AddTaskRateViewModel model)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Docente")
            {
                return RedirectToAction("Index", "Home");
            }

            if (model.Calificacion > 0 && model.Calificacion <= 10)
            {
                arProyectoBO.AddTaskRate(model);
            }
            else
            {
                ViewBag.Error = "Ingrese un número entre 1 y 10";
            }

            var addTaskRate = new AddTaskRateViewModel();

            addTaskRate.CourseId = model.CourseId;
            addTaskRate.TaskId = model.TaskId;

            return RedirectToAction("TaskRateTeacher", addTaskRate);
        }

        public IActionResult TaskRateStudent(int courseId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }
            var addTaskRate = new AddTaskRateStudentViewModel();

            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));

            addTaskRate.Tareas = arProyectoBO.FindCourseTaskRates(courseId, userId);
            addTaskRate.CourseId = courseId;
            addTaskRate.UsuarioId = userId;

            return View(addTaskRate);
        }

        [HttpPost]
        public IActionResult TaskRateStudent(AddTaskRateStudentViewModel model)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Estudiante")
            {
                return RedirectToAction("Index", "Home");
            }
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            

            if (model.Calificacion > 0 && model.Calificacion <= 10)
            {
                arProyectoBO.AddUsuarioTaskRate(model);
            }
            else
            {
                ViewBag.Error = "Ingrese un número entre 1 y 10";
            }

            var addTaskRate = new AddTaskRateStudentViewModel();
            addTaskRate.CourseId = model.CourseId;

            return RedirectToAction("TaskRateStudent", addTaskRate);
        }
    }
}
