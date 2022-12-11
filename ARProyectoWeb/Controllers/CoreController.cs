﻿using ARProyectoWeb.Business.BO;
using ARProyectoWeb.Business.Models;
using ARProyectoWeb.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace ARProyectoWeb.Controllers
{
    public class CoreController : Controller
    {
        ARProyectoBO arProyectoBO = new ARProyectoBO();

        public IActionResult CourseEngagement(int courseId)
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
            List<EngagementInformationViewModel> engagementInformation = new List<EngagementInformationViewModel>();

            if (courseId != 0)
            {
                engagementInformation = arProyectoBO.GetCourseEngagement(courseId);
            }
            return View(engagementInformation);
        }

        public IActionResult TaskEngagement(int taskId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var tasks = new List<Data.Models.Task>();
            if (userRole == "Admin")
            {
                tasks = arProyectoBO.FindTasks();
            }
            else if (userRole == "Docente" || userRole == "Estudiante")
            {
                var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                tasks = arProyectoBO.FindUsuarioTasks(userId);
            }

            ViewBag.TaskId = new SelectList(tasks, "TaskId", "Titulo");
            List<EngagementInformationViewModel> engagementInformation = new List<EngagementInformationViewModel>();

            if (taskId != 0)
            {
                engagementInformation = arProyectoBO.GetTaskEngagement(taskId);
            }
            return View(engagementInformation);
            
        }
    }
}
