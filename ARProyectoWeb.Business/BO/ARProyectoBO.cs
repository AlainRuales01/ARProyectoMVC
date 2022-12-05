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
        public Usuario FindUserById(int usuarioId)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var usuario = _context.Usuario.Find(usuarioId);
                return usuario;
            }
            
        }

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

        public void AddNewUser(Usuario nuevoUsuario)
        {
            using(DataBaseContext _context = new DataBaseContext())
            {
                _context.Usuario.Add(nuevoUsuario);
                _context.SaveChanges();
            }
        }

        public void EditUser(Usuario usuarioModificar)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                _context.Entry(usuarioModificar).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public List<Course> FindCourses()
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var cursos = _context.Course.ToList();
                return cursos;
            }
        }

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

        public Course FindCourseById(int courseId)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                var curso = _context.Course.Find(courseId);
                return curso;
            }

        }

        public void AddNewCourse(Course nuevoCurso)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                _context.Course.Add(nuevoCurso);
                _context.SaveChanges();
            }
        }

        public void EditCourse(Course cursoModificar)
        {
            using (DataBaseContext _context = new DataBaseContext())
            {
                _context.Entry(cursoModificar).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

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
