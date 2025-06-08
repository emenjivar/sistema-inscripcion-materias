namespace InscripcionMaterias.Models.ViewModels
{
    public class InscripcionViewModel
    {
        public string NombreAlumno { get; set; } = "";
        public string Carnet { get; set; } = "";
        public int CicloAcademico { get; set; }
        public int Anio { get; set; }

        public List<Materium> MateriasDisponibles { get; set; } = new();
        public List<GrupoClase> GruposDisponibles { get; set; } = new();
        public List<InscripcionSeleccionadaViewModel> MateriasSeleccionadas { get; set; } = new();
        public List<GrupoMateriaHorarioViewModel> GrupoMateriaHorarios { get; set; } = new();
    }

    public class InscripcionSeleccionadaViewModel
    {
        public string MateriaNombre { get; set; } = "";
        public int IdBloqueHorarioMateria { get; set; }
        public string Grupo { get; set; } = "";
        public List<string> Horarios { get; set; } = new();
    }
}
