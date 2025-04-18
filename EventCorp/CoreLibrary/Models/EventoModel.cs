using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    public class EventoModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
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
        public CategoriaModel Categoria { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha no válido.")]
        [FutureDate(ErrorMessage = "La fecha del evento debe ser en el futuro.")]
        [Display(Name = "Fecha del Evento")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria.")]
        [Display(Name = "Hora del Evento")]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser mayor a cero.")]
        [Display(Name = "Duración (minutos)")]
        public int Duracion { get; set; }

        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        [StringLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres.")]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }

        [Required(ErrorMessage = "El cupo máximo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser al menos 1.")]
        [Display(Name = "Cupo Máximo")]
        public int CupoMaximo { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }

        [Display(Name = "Usuario que Registró")]
        public int UsuarioRegistroId { get; set; }

        public UsuarioModel UsuarioRegistro { get; set; }
    }
}
