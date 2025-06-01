using System.ComponentModel.DataAnnotations;

namespace InscripcionMaterias.CustomModels
{
    /**
     * Clase usada para editar usuarios.
     * A diferencia de Usuario.cs, en esta clase el atributo 'password' es nullalizable,
     * lo cual nos permite no enviar el password desde el formulario y el repository lo interpreta
     * como un dato que no debe actualizar.
     */
    public class UsuarioEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        public string Email { get; set; } = null!;

        public string? Nombre { get; set; }

        // La nueva controsena es opcional durante la edicion
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La nueva contraseña debe tener al menos 6 caracteres.")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage ="El rol es obligatorio.")]
        public string Rol { get; set; } = null!;
    }
}
