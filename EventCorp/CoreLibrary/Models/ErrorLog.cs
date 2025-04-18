using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    public class ErrorLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? UsuarioId { get; set; }

        [MaxLength(500)]
        public string Mensaje { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? StackTrace { get; set; }

        [MaxLength(200)]
        public string Origen { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Tipo { get; set; } = "Error"; // Ej: "Error", "Warning", "Info"

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [ForeignKey("UsuarioId")]
        public UsuarioModel? Usuario { get; set; }

    }
}
