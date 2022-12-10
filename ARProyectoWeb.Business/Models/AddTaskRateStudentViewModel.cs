using ARProyectoWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARProyectoWeb.Business.Models
{
    public class AddTaskRateStudentViewModel
    {
        public int CourseId { get; set; }
        public int TaskId { get; set; }
        public int UsuarioId { get; set; }
        public List<TaskModel> Tareas{ get; set; }
        public double Calificacion { get; set; }
    }

    public class TaskModel
    {
        public int TaskId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public double Calificacion { get; set; }
        public double CalificacionUsuario { get; set; }
    }
}
