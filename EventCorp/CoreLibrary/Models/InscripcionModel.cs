using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Models
{
    public class InscripcionModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int EventoId { get; set; }

        [Required]
        public DateTime FechaInscripcion { get; set; }

        public bool Asistio { get; set; }

        // Propiedades de navegación
        public virtual UsuarioModel Usuario { get; set; }
        public virtual EventoModel Evento { get; set; }
    }
}
