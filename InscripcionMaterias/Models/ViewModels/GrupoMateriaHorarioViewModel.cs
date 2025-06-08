namespace InscripcionMaterias.Models.ViewModels
{
    public class GrupoMateriaHorarioViewModel
    {
        public int IdBloqueHorarioMateria { get; set; }
        public string MateriaNombre { get; set; } = "";
        public string GrupoCodigo { get; set; } = "";
        public string HorarioDescripcion { get; set; } = "";
    }

}
