using Microsoft.AspNetCore.Mvc.Rendering;

namespace InscripcionMaterias.Models.ViewModels
{
    public class FiltroCierreCicloViewModel
    {
        public int? IdPensum { get; set; }
        public int? CicloAcademico { get; set; }
        public int? Anio { get; set; }

        public List<SelectListItem> Pensums { get; set; } = new();
        public List<SelectListItem> Ciclos { get; set; } = new();
        public List<SelectListItem> Anios { get; set; } = new();

        public List<CierreCicloViewModel> Resultados { get; set; } = new();
    }


}
