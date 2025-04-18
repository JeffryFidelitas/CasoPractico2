using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Auth;

namespace CoreLibrary.Models
{
    public class UsuarioModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El nombre de usuario solo puede contener letras y números.")]
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre completo no puede exceder los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre completo solo puede contener letras y espacios.")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "El correo electrónico no es válido.")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "El número de teléfono no es válido.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula y un número.")]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        public RolesEnum Rol { get; set; }

        public ICollection<CategoriaModel> CategoriasRegistradas { get; set; } = new List<CategoriaModel>();
        public ICollection<EventoModel> EventosRegistrados { get; set; } = new List<EventoModel>();
        public ICollection<ErrorLog> Errores { get; set; } = new List<ErrorLog>();
    }
}
