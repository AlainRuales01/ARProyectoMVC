using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARProyectoWeb.Data.Models
{
    public class TaskCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskCourseId { get; set; }
        public int CourseId { get; set; }
        public int TaskId { get; set; }
        public double CalificacionProfesor { get; set; }
        public Course Curso{ get; set; }
        public Task Tarea { get; set; }
        public List<TaskRate> CalificacionesTarea { get; set; }
    }
}
