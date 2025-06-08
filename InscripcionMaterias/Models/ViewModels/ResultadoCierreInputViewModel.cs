namespace InscripcionMaterias.Models.ViewModels
{
    public class ResultadoCierreInputViewModel
    {
        public int IdPensum { get; set; }
        public int Ciclo { get; set; }
        public int Anio { get; set; }

        public List<ResultadoAlumnoViewModel> Resultados { get; set; } = new();
    }

    public class ResultadoAlumnoViewModel
    {
        public int IdAlumno { get; set; }
        public List<int> MateriasAprobadas { get; set; } = new(); // IDs de materias seleccionadas
    }

}
