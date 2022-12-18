using ARProyectoWeb.Business.Models;
using ARProyectoWeb.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ARProyectoWeb.Business.BO
{
    public class ARProyectoBO
    {
        /// <summary>
        /// Devuelve un usuario por Id especificado
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public Usuario FindUsuarioById(int usuarioId)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var usuario = _context.Usuario.Find(usuarioId);
                return usuario;
            }

        }

        /// <summary>
        /// Devuelve la lista de usuarios a los que puede acceder un rol de usuario
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public List<Usuario> FindUsuariosList(string userRole)
        {
            var usuarios = new List<Usuario>();
            using (DataBaseContext _context = new DataBaseContext())
            {
                if (userRole == "Admin")
                {
                    usuarios = _context.Usuario.Where(u => u.Rol != "Admin").ToList();
                }
                else if (userRole == "Docente")
                {
                    usuarios = _context.Usuario.Where(u => u.Rol != "Admin" && u.Rol != "Docente").ToList();
                }
            }
            return usuarios;
        }

        /// <summary>
        /// Devuelve todos los docentes creados
        /// </summary>
        /// <returns></returns>
        public List<Usuario> FindDocentes()
        {
            var docentes = new List<Usuario>();
            using (DataBaseContext _context = new DataBaseContext())
            {
                docentes = _context.Usuario.Where(u => u.Rol == "Docente").ToList();
            }
            return docentes;
        }

        /// <summary>
        /// Agrega nuevo usuario
        /// </summary>
        /// <param name="nuevoUsuario"></param>
        public void AddNewUsuario(Usuario nuevoUsuario)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                _context.Usuario.Add(nuevoUsuario);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Edita un usuario creado
        /// </summary>
        /// <param name="usuario"></param>
        public void EditUsuario(Usuario usuario)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var usuarioModificar = _context.Usuario.Find(usuario.UsuarioId);
                if (usuarioModificar != null)
                {
                    usuarioModificar.Nombres = usuario.Nombres;
                    usuarioModificar.Correo = usuario.Correo;
                    _context.Entry(usuarioModificar).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Devuelve todos los cursos de la base de datos
        /// </summary>
        /// <returns></returns>
        public List<Course> FindCourses()
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var cursos = _context.Course.ToList();
                return cursos;
            }
        }

        /// <summary>
        /// Busca todos los cursos a los que pertenece un usuario
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public List<Course> FindUsuarioCourses(int usuarioId)
        {
            List<Course> cursos = new List<Course>();
            using (DataBaseContext _context = new DataBaseContext())
            {
                var usuarioCoursesId = _context.UsuarioCourse.Where(u => u.UsuarioId == usuarioId).Select(c => c.CourseId).Distinct().ToList();
                cursos = _context.Course.Where(c => usuarioCoursesId.Contains(c.CourseId)).ToList();
            }
            return cursos;
        }

        /// <summary>
        /// Busca los usuarios que pertenecen a un curso
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Usuario> FindCourseUsuarios(int courseId)
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (DataBaseContext _context = new DataBaseContext())
            {
                var usuariosCourseId = _context.UsuarioCourse.Where(c => c.CourseId == courseId).Select(c => c.UsuarioId).Distinct().ToList();
                usuarios = _context.Usuario.Where(u => usuariosCourseId.Contains(u.UsuarioId)).ToList();
            }
            return usuarios;
        }

        /// <summary>
        /// Busca las tasks que pertenecen a un curso
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Data.Models.Task> FindCourseTasks(int courseId)
        {
            List<Data.Models.Task> tasks = new List<Data.Models.Task>();
            using (DataBaseContext _context = new DataBaseContext())
            {
                var taskCourseId = _context.TaskCourse.Where(c => c.CourseId == courseId).Select(c => c.TaskId).ToList();
                tasks = _context.Task.Where(u => taskCourseId.Contains(u.TaskId)).ToList();
            }
            return tasks;
        }

        /// <summary>
        /// Devuelve un curso en base al Id especificado
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public Course FindCourseById(int courseId)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var curso = _context.Course.Find(courseId);
                return curso;
            }

        }

        /// <summary>
        /// Agrega nuevo curso
        /// </summary>
        /// <param name="nuevoCurso"></param>
        public void AddNewCourse(Course nuevoCurso)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                _context.Course.Add(nuevoCurso);
                _context.SaveChanges();
            }
        }
        /// <summary>
        /// Edita un curso creado
        /// </summary>
        /// <param name="curso"></param>
        public void EditCourse(Course curso)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var cursoModificar = _context.Course.Find(curso.CourseId);
                if (cursoModificar != null)
                {
                    cursoModificar.Nombre = curso.Nombre;
                    cursoModificar.Descripcion = curso.Descripcion;
                    _context.Entry(cursoModificar).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Agrega un usuario a un curso
        /// </summary>
        /// <param name="usuarioCourse"></param>
        public void AddUsuarioCourse(UsuarioCourse usuarioCourse)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var usuarioCourseExistente = _context.UsuarioCourse.Where(u => u.UsuarioId == usuarioCourse.UsuarioId && u.CourseId == usuarioCourse.CourseId).FirstOrDefault();
                if (usuarioCourseExistente == null)
                {
                    _context.UsuarioCourse.Add(usuarioCourse);
                    _context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Agrega una task a un curso
        /// </summary>
        /// <param name="taskCourse"></param>
        public void AddTaskCourse(TaskCourse taskCourse)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var taskCourseExistente = _context.TaskCourse.Where(t => t.TaskId == taskCourse.TaskId && t.CourseId == taskCourse.CourseId).FirstOrDefault();
                if (taskCourseExistente == null)
                {
                    _context.TaskCourse.Add(taskCourse);
                    _context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Devuelve una task en base a un Id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Data.Models.Task FindTaskById(int taskId)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var task = _context.Task.Find(taskId);
                return task;
            }

        }

        /// <summary>
        /// Devuelve todas las tareas creadas
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public List<Data.Models.Task> FindTasks()
        {
            List<Data.Models.Task> tasks = new List<Data.Models.Task>();

            using (DataBaseContext _context = new DataBaseContext())
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
            return tasks;
        }

        /// <summary>
        /// Agrega una nueva task
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public void AddNewTask(Data.Models.Task nuevoTask, int usuarioId)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var usuario = _context.Usuario.Where(u => u.UsuarioId == usuarioId).FirstOrDefault();

                if (usuario.Rol == "Docente")
                {
                    nuevoTask.UsuarioId = usuario.UsuarioId;
                }

                _context.Task.Add(nuevoTask);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Edita una task
        /// </summary>
        /// <param name="task"></param>
        public void EditTask(Data.Models.Task task)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var tareaModificar = _context.Task.Find(task.TaskId);
                if (tareaModificar != null)
                {
                    tareaModificar.Titulo = task.Titulo;
                    tareaModificar.Descripcion = task.Descripcion;
                    tareaModificar.UsuarioId = task.UsuarioId;
                    _context.Entry(tareaModificar).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Devuelve las tareas creadas por el usuario especificado
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public List<Data.Models.Task> FindUsuarioTasks(int usuarioId)
        {
            List<Data.Models.Task> tasks = new List<Data.Models.Task>();

            using (DataBaseContext _context = new DataBaseContext())
            {
                tasks = (from t in _context.Task
                         join us in _context.Usuario on t.UsuarioId equals us.UsuarioId
                         where t.UsuarioId == usuarioId
                         select new Data.Models.Task
                         {
                             TaskId = t.TaskId,
                             UsuarioId = t.UsuarioId,
                             Titulo = t.Titulo,
                             Descripcion = t.Descripcion,
                             UsuarioCreador = us
                         }).ToList();
            }
            return tasks;
        }

        /// <summary>
        /// Obtiene los estudiantes con sus respectivas calificaciones en las diferentes tareas asignadas a un curso
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public List<UsuarioModel> FindUsuarioTaskRates(int courseId, int taskId)
        {
            var usuariosModel = new List<UsuarioModel>();
            using (DataBaseContext _context = new DataBaseContext())
            {
                var usuariosCourseId = _context.UsuarioCourse.Where(c => c.CourseId == courseId).Select(u => u.UsuarioId).ToList();
                var usuarios = _context.Usuario.Where(u => usuariosCourseId.Contains(u.UsuarioId) && u.Rol != "Docente").ToList();

                var taskCourses = _context.TaskCourse.Where(t => t.CourseId == courseId && t.TaskId == taskId).FirstOrDefault();
                var taskRates = _context.TaskRate.Where(t => t.TaskCourseId == taskCourses.TaskCourseId).ToList();

                foreach (var item in usuarios)
                {
                    var usuarioModel = new UsuarioModel();

                    usuarioModel.UsuarioId = item.UsuarioId;
                    usuarioModel.Nombres = item.Nombres;
                    usuarioModel.Apellidos = item.Apellidos;
                    usuarioModel.Correo = item.Correo;
                    usuarioModel.FechaNacimiento = item.FechaNacimiento;

                    var usuarioTaskRate = taskRates.Where(t => t.UsuarioId == item.UsuarioId).FirstOrDefault();

                    if (usuarioTaskRate != null)
                    {
                        usuarioModel.Calificacion = usuarioTaskRate.Calificacion;
                        usuarioModel.CalificacionUsuario = usuarioTaskRate.CalificacionUsuario;
                    }

                    usuariosModel.Add(usuarioModel);

                }
            }
            return usuariosModel;
        }

        /// <summary>
        /// Obtiene las tareas de un curso con sus las calificaciones de las tareas para un usuario
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public List<TaskModel> FindCourseTaskRates(int courseId, int usuarioId)
        {
            var tasksModel = new List<TaskModel>();
            //var usuariosModel = new List<UsuarioModel>();
            using (DataBaseContext _context = new DataBaseContext())
            {
                var taskCourse = _context.TaskCourse.Where(c => c.CourseId == courseId).ToList();

                var tasks = (from tc in taskCourse
                             join t in _context.Task on tc.TaskId equals t.TaskId
                             select t).ToList();


                var taskRates = (from tc in taskCourse
                                 join tr in _context.TaskRate on tc.TaskCourseId equals tr.TaskCourseId
                                 where tr.UsuarioId == usuarioId
                                 select new
                                 {
                                     TaskId = tc.TaskId,
                                     Calificacion = tr.Calificacion,
                                     CalificacionUsuario = tr.CalificacionUsuario
                                 }).ToList();

                foreach (var item in tasks)
                {
                    var taskModel = new TaskModel();

                    taskModel.TaskId = item.TaskId;
                    taskModel.Titulo = item.Titulo;
                    taskModel.Descripcion = item.Descripcion;

                    var taskRate = taskRates.Where(t => t.TaskId == item.TaskId).FirstOrDefault();

                    if (taskRate != null)
                    {
                        taskModel.Calificacion = taskRate.Calificacion;
                        taskModel.CalificacionUsuario = taskRate.CalificacionUsuario;
                    }

                    tasksModel.Add(taskModel);

                }
            }
            return tasksModel;
        }



        /// <summary>
        /// Agrega una calificación del profesor hacia un estudiante en una tarea
        /// </summary>
        /// <param name="model"></param>
        public void AddTaskRate(AddTaskRateViewModel model)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var taskCourse = _context.TaskCourse.Where(t => t.CourseId == model.CourseId && t.TaskId == model.TaskId).FirstOrDefault();

                TaskRate taskRate = _context.TaskRate.Where(t => t.TaskCourseId == taskCourse.TaskCourseId && t.UsuarioId == model.UsuarioId).FirstOrDefault();

                if (taskRate == null)
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
            }
        }

        /// <summary>
        /// Agrega una calificacion de un usuario hacia una tarea
        /// </summary>
        /// <param name="model"></param>
        public void AddUsuarioTaskRate(AddTaskRateStudentViewModel model)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var taskCourse = _context.TaskCourse.Where(t => t.CourseId == model.CourseId && t.TaskId == model.TaskId).FirstOrDefault();

                TaskRate taskRate = _context.TaskRate.Where(t => t.TaskCourseId == taskCourse.TaskCourseId && t.UsuarioId == model.UsuarioId).FirstOrDefault();

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
            }
        }

        /* Código destinado a CORE */

        public List<EngagementInformationViewModel> GetCourseEngagement(int courseId)
        {

            List<EngagementInformationViewModel> engagementInformation = new List<EngagementInformationViewModel>();

            using (DataBaseContext _context = new DataBaseContext())
            {
                var taskCourse = _context.TaskCourse.Where(c => c.CourseId == courseId).ToList();

                var tasks = (from tc in taskCourse
                             join t in _context.Task on tc.TaskId equals t.TaskId
                             select t).ToList();

                var taskRates = (from tc in taskCourse
                                 join tr in _context.TaskRate on tc.TaskCourseId equals tr.TaskCourseId
                                 select new
                                 {
                                     TaskId = tc.TaskId,
                                     Calificacion = tr.Calificacion,
                                     CalificacionUsuario = tr.CalificacionUsuario
                                 }).ToList();

                foreach (var item in tasks)
                {
                    double sumaCalificaciones = 0;

                    var calificaciones = taskRates.Where(t => t.TaskId == item.TaskId).Select(t => t.Calificacion).ToList();

                    var cantCalificaciones = calificaciones.Count;

                    double sumaCalificacionesEstudiante = 0;

                    var calificacionesEstudiante = taskRates.Where(t => t.TaskId == item.TaskId).Select(t => t.CalificacionUsuario).ToList();

                    var cantCalificacionesEstudiante = calificacionesEstudiante.Count;

                    var engagement = new EngagementInformationViewModel();

                    double calificacionProfesor = taskCourse.Where(t => t.TaskId == item.TaskId).Select(t => t.CalificacionProfesor).FirstOrDefault();

                    engagement.Nombre = item.Titulo;

                    foreach (var calificacion in calificaciones)
                    {
                        sumaCalificaciones += calificacion;
                    }

                    foreach (var calificacion in calificacionesEstudiante)
                    {
                        sumaCalificacionesEstudiante += calificacion;
                    }

                    var calificacionpromedio = Math.Round(sumaCalificaciones / cantCalificaciones, 2);
                    var calificacionUsuariopromedio = Math.Round(sumaCalificacionesEstudiante / cantCalificacionesEstudiante, 2);

                    engagement.CalificacionProfesor = calificacionProfesor;
                    engagement.CalificacionPromedio = calificacionpromedio;
                    engagement.CalificacionUsuarioPromedio = calificacionUsuariopromedio;
                    engagement.CalificacionEngagement = Math.Round((calificacionProfesor + calificacionpromedio + calificacionUsuariopromedio) / 3, 2);

                    engagementInformation.Add(engagement);
                }
            }

            return engagementInformation;
        }

        public List<EngagementInformationViewModel> GetTaskEngagement(int taskId)
        {

            List<EngagementInformationViewModel> engagementInformation = new List<EngagementInformationViewModel>();

            using (DataBaseContext _context = new DataBaseContext())
            {
                var taskCourse = _context.TaskCourse.Where(c => c.TaskId == taskId).ToList();

                var courses = (from tc in taskCourse
                               join t in _context.Course on tc.CourseId equals t.CourseId
                               select t).ToList();

                var taskRates = (from tc in taskCourse
                                 join tr in _context.TaskRate on tc.TaskCourseId equals tr.TaskCourseId
                                 select new
                                 {
                                     CourseId = tc.CourseId,
                                     Calificacion = tr.Calificacion,
                                     CalificacionUsuario = tr.CalificacionUsuario
                                 }).ToList();

                foreach (var item in courses)
                {
                    double sumaCalificaciones = 0;

                    var calificaciones = taskRates.Where(t => t.CourseId == item.CourseId).Select(t => t.Calificacion).ToList();

                    var cantCalificaciones = calificaciones.Count;

                    double sumaCalificacionesEstudiante = 0;

                    var calificacionesEstudiante = taskRates.Where(t => t.CourseId == item.CourseId).Select(t => t.CalificacionUsuario).ToList();

                    var cantCalificacionesEstudiante = calificacionesEstudiante.Count;

                    var engagement = new EngagementInformationViewModel();

                    double calificacionProfesor = taskCourse.Where(t => t.CourseId == item.CourseId).Select(t => t.CalificacionProfesor).FirstOrDefault();

                    engagement.Nombre = item.Nombre;

                    foreach (var calificacion in calificaciones)
                    {
                        sumaCalificaciones += calificacion;
                    }

                    foreach (var calificacion in calificacionesEstudiante)
                    {
                        sumaCalificacionesEstudiante += calificacion;
                    }

                    var calificacionpromedio = Math.Round(sumaCalificaciones / cantCalificaciones, 2);
                    var calificacionUsuariopromedio = Math.Round(sumaCalificacionesEstudiante / cantCalificacionesEstudiante, 2);

                    engagement.CalificacionProfesor = calificacionProfesor;
                    engagement.CalificacionPromedio = calificacionpromedio;
                    engagement.CalificacionUsuarioPromedio = calificacionUsuariopromedio;
                    engagement.CalificacionEngagement = Math.Round((calificacionProfesor + calificacionpromedio + calificacionUsuariopromedio) / 3, 2);

                    engagementInformation.Add(engagement);
                }
            }

            return engagementInformation;
        }

        public List<EngagementInformationViewModel> GetCourseCategoryEngagement(string courseCategory)
        {
            List<EngagementInformationViewModel> engagementInformation = new List<EngagementInformationViewModel>();

            using (DataBaseContext _context = new DataBaseContext())
            {

                var listCourses = (from c in _context.Course
                                   where c.Categoria == courseCategory
                                   select c).Distinct().ToList();

                var listCoursesId = listCourses.Select(c => c.CourseId).ToList();

                var taskCourse = _context.TaskCourse.Where(c => listCoursesId.Contains(c.CourseId)).ToList();

                var tasks = (from tc in taskCourse
                             join t in _context.Task on tc.TaskId equals t.TaskId
                             select t).Distinct().ToList();

                var taskRates = (from tc in taskCourse
                                 join tr in _context.TaskRate on tc.TaskCourseId equals tr.TaskCourseId
                                 select new
                                 {
                                     TaskId = tc.TaskId,
                                     TaskCourseId = tc.TaskCourseId,
                                     Calificacion = tr.Calificacion,
                                     CalificacionUsuario = tr.CalificacionUsuario
                                 }).ToList();

                foreach (var item in tasks)
                {
                    var courses = taskCourse.Where(t => t.TaskId == item.TaskId).ToList();

                    foreach (var itemCourse in courses)
                    {
                        var taskCourseInfo = taskCourse.Where(t => t.TaskId == item.TaskId && t.CourseId == itemCourse.CourseId).FirstOrDefault();

                        double sumaCalificaciones = 0;

                        var calificaciones = taskRates.Where(t => t.TaskId == item.TaskId && t.TaskCourseId ==  taskCourseInfo.TaskCourseId).Select(t => t.Calificacion).ToList();

                        var cantCalificaciones = calificaciones.Count;

                        double sumaCalificacionesEstudiante = 0;

                        var calificacionesEstudiante = taskRates.Where(t => t.TaskId == item.TaskId && t.TaskCourseId == taskCourseInfo.TaskCourseId).Select(t => t.CalificacionUsuario).ToList();

                        var cantCalificacionesEstudiante = calificacionesEstudiante.Count;

                        var engagement = new EngagementInformationViewModel();


                        var course = listCourses.Where(c => c.CourseId == taskCourseInfo.CourseId).FirstOrDefault();

                        double calificacionProfesor = taskCourseInfo.CalificacionProfesor;

                        engagement.NombreCourse = course.Nombre;
                        engagement.Nombre = item.Titulo;

                        foreach (var calificacion in calificaciones)
                        {
                            sumaCalificaciones += calificacion;
                        }

                        foreach (var calificacion in calificacionesEstudiante)
                        {
                            sumaCalificacionesEstudiante += calificacion;
                        }

                        var calificacionpromedio = Math.Round(sumaCalificaciones / cantCalificaciones, 2);
                        var calificacionUsuariopromedio = Math.Round(sumaCalificacionesEstudiante / cantCalificacionesEstudiante, 2);

                        engagement.CalificacionProfesor = calificacionProfesor;
                        engagement.CalificacionPromedio = calificacionpromedio;
                        engagement.CalificacionUsuarioPromedio = calificacionUsuariopromedio;
                        engagement.CalificacionEngagement = Math.Round((calificacionProfesor + calificacionpromedio + calificacionUsuariopromedio) / 3, 2);

                        engagementInformation.Add(engagement);
                    }
                }
            }

            return engagementInformation;
        }

    }
}
