using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARProyectoWeb.Data.Models
{
    public class UsuarioCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioCourseId { get; set; }
        public int UsuarioId { get; set; }
        public int CourseId { get; set; }
        public Usuario Usuario { get; set; }
        public Course Curso { get; set; }

    }
}
