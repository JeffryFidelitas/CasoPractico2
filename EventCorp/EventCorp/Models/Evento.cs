using EventCorp.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace EventCorp.Models
{
    public class Evento
    {
        [Key]
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(150)]
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "La descripción es campo obligatorio")]
        [Display(Name = "Descripción")]
        [StringLength(1000)]
        public string Descripcion { get; set; }
        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId { get; set; }

        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "La fecha del evento no puede estar en el pasado")]
        [Required(ErrorMessage = "La fecha del evento es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [PositiveDuration(ErrorMessage = "La duración debe ser mayor a cero")]
        [Display(Name = "Duración en minutos")]
        public int Duracion { get; set; }

        [Display(Name = "Ubicación")]
        [StringLength(200)]
        public string Ubicacion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor a 0")]
        public int MaxAsistentes { get; set; }

        public DateTime FechaRegistro { get; set; }

        //public int? UsuarioId { get; set; }
        //[Display(Name = "Usuario de Registro")]
        //public virtual Usuario? Usuario { get; set; }

        // Relación con Categoría
        public Categoria? Categoria { get; set; }
    }
}
