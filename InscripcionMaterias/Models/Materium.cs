using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;

public partial class Materium
{
    public int Id { get; set; }


    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public int UnidadesValorativas { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<BloqueHorarioMaterial> BloqueHorarioMaterials { get; set; } = new List<BloqueHorarioMaterial>();

    public virtual ICollection<PensumMateria> PensumMateriaIdMateriaNavigations { get; set; } = new List<PensumMateria>();

    public virtual ICollection<PensumMateria> PensumMateriaIdMateriaPrerequisitoNavigations { get; set; } = new List<PensumMateria>();

    public virtual ICollection<ResultadoCicloAcademico> ResultadoCicloAcademicos { get; set; } = new List<ResultadoCicloAcademico>();
}
