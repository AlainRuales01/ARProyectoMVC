using ARProyectoWeb.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Usuario FindUserById(int usuarioId)
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
        public List<Usuario> FindUsersList(string userRole)
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
                else
                {

                    // Retornar error
                }
            }
            return usuarios;
        }

        /// <summary>
        /// Agrega nuevo usuario
        /// </summary>
        /// <param name="nuevoUsuario"></param>
        public void AddNewUser(Usuario nuevoUsuario)
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
        public void EditUser(Usuario usuario)
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
        /// Busca los usuarios que pertenecen a un curso
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
        /// <param name="cursoModificar"></param>
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
        public void AddUserCourse(UsuarioCourse usuarioCourse)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                _context.UsuarioCourse.Add(usuarioCourse);
                _context.SaveChanges();
            }
        }
    }
}
