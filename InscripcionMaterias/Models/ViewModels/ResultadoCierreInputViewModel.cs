namespace InscripcionMaterias.Models.ViewModels
{
    public class ResultadoCierreInputViewModel
    {
        public int IdPensum { get; set; }
        public int Ciclo { get; set; }
        public int Anio { get; set; }
        public List<ResultadoAlumnoVM> Resultados { get; set; } = new();
    }

    public class ResultadoAlumnoVM
    {
        public int IdAlumno { get; set; }

        // Lista de materias con su estado (aprobado o reprobado)
        public List<MateriaResultadoVM> Materias { get; set; } = new();
    }

    public class MateriaResultadoVM
    {
        public int IdMateria { get; set; }
        public bool Aprobado { get; set; } // true = aprobado, false = reprobado
    }


}
