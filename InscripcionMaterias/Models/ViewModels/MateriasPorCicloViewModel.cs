using InscripcionMaterias.Models;

namespace InscripcionMaterias.Models.ViewModels
{
    public class MateriasPorCicloViewModel
    {
        public int Año { get; set; }

        // Diccionario que tiene como clave el ciclo y como valor la lista de materias de ese ciclo
        public Dictionary<int, List<PensumMateria>> MateriasPorCiclo { get; set; } = new Dictionary<int, List<PensumMateria>>();

    }
}
