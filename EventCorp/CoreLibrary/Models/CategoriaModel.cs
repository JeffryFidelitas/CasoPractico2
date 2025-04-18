using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    public class CategoriaModel
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es un campo obligatorio.")]
        [StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }

        [ScaffoldColumn(false)]
        public int UsuarioRegistroId { get; set; }

        [ForeignKey(nameof(UsuarioRegistroId))]
        public UsuarioModel UsuarioRegistro { get; set; }

        public virtual ICollection<EventoModel> Eventos { get; set; } = new List<EventoModel>();

        public CategoriaModel()
        {
            FechaRegistro = DateTime.Now;
        }
    }
}
