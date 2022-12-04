using ARProyectoWeb.Data.Models;

namespace ARProyectoWeb.ViewModels
{
    public class AddTaskRateViewModel
    {
        public int CourseId { get; set; }
        public int TaskId { get; set; }
        public int UsuarioId { get; set; }
        public List<UsuarioModel> Estudiantes { get; set; }
        public List<TaskRate> CalificacionesEstudiantes { get; set; }
        public double Calificacion { get; set; }
    }

    public class UsuarioModel
    {
        public int UsuarioId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public double Calificacion { get; set; }
    }
}
