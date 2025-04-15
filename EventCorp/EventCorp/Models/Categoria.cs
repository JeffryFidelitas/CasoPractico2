using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace EventCorp.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La descripción es un campo obligatorio.")]
        [Display(Name = "Descripción")]
        [StringLength(200)]
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }
        //public int? UsuarioId { get; set; }
        //[Display(Name = "Usuario de Registro")]
        //public virtual Usuario? Usuario { get; set; }
        public virtual ICollection<Evento>? Eventos { get; set; }
    }
}
