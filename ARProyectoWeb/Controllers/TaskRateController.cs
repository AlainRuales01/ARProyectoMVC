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
            var addTaskRate = new AddTaskRateViewModel();

            addTaskRate.Estudiantes = arProyectoBO.FindUsuarioTaskRates(courseId, taskId);
            addTaskRate.CourseId = courseId;
            addTaskRate.TaskId = taskId;

            return View(addTaskRate);
        }

        [HttpPost]
        public IActionResult TaskRateTeacher(AddTaskRateViewModel model)
        {
            var addTaskRate = new AddTaskRateViewModel();
            addTaskRate.Estudiantes = arProyectoBO.FindUsuarioTaskRates(model.CourseId, model.TaskId);
            addTaskRate.CourseId = model.CourseId;
            addTaskRate.TaskId = model.TaskId;

            arProyectoBO.AddTaskRate(model);

            return View(addTaskRate);
        }

        public IActionResult TaskRateStudent(int courseId)
        {
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
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            var addTaskRate = new AddTaskRateStudentViewModel();

            addTaskRate.Tareas = arProyectoBO.FindCourseTaskRates(model.CourseId, userId);
            addTaskRate.CourseId = model.CourseId;
            addTaskRate.UsuarioId = userId;

            arProyectoBO.AddUsuarioTaskRate(model);

            return View();
        }
    }
}
