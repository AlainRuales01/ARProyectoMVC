using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARProyectoWeb.Data.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }
        public int UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Usuario UsuarioCreador { get; set; }
        public List<TaskCourse> CursosTarea { get; set; }
    }
}
