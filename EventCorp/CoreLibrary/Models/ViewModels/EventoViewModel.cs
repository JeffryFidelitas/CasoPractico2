using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Models.ViewModels
{
    public class EventoViewModel
    {
        // Reutilizando las validaciones del modelo Evento
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(150, ErrorMessage = "El título no puede exceder los 150 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "La fecha del evento no puede estar en el pasado.")]
        [Display(Name = "Fecha del Evento")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria.")]
        [DataType(DataType.Time)]
        public string Hora { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser mayor a cero.")]
        [Display(Name = "Duración (minutos)")]
        public int Duracion { get; set; }

        [StringLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres.")]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor a 0.")]
        [Display(Name = "Cupo Máximo")]
        public int MaxAsistentes { get; set; }

        // Puedes mantener el valor de FechaRegistro y UsuarioRegistro en el ViewModel también
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }

        // Relación con la categoría y la entidad Usuario (si lo necesitas)
        public CategoriaModel? Categoria { get; set; }
    }
}
