using System.ComponentModel.DataAnnotations;

namespace InscripcionMaterias.Models.ViewModels
{
    public class GrupoClaseViewModel
    {
        public int Id { get; set; } // Será 0 para un nuevo grupo, o el ID para uno existente

        [Required(ErrorMessage = "El código del grupo es obligatorio.")]
        [StringLength(10, ErrorMessage = "El código no puede exceder los 10 caracteres.")]
        public string Codigo { get; set; } = null!;
    }
}
