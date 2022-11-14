using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARProyectoWeb.Data.Models
{
    public class TaskRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskRateId { get; set; }
        public int UsuarioId { get; set; }
        public int TaskCourseId { get; set; }
        public double Calificacion{ get; set; }
        public double CalificacionUsuario { get; set; }
        public Usuario Estudiante { get; set; }
        public TaskCourse Tarea { get; set; }
    }
}
