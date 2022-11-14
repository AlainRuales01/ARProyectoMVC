using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARProyectoWeb.Data.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string Rol { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<UsuarioCourse> Cursos { get; set; }
        public List<Task> TareasCreadas { get; set; }
        public List<TaskRate> CalificacionesEstudiante{ get; set; }

    }
}
