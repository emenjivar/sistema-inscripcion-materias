namespace InscripcionMaterias.Models.ViewModels
{
    public class CierreCicloViewModel
    {
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; } = string.Empty;

        public List<MateriaCierreVM> Materias { get; set; } = new();
    }

    public class MateriaCierreVM
    {
        public int IdMateria { get; set; }
        public string NombreMateria { get; set; } = string.Empty;
        public bool Aprobado { get; set; }
    }

}
