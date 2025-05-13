using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;

public partial class GrupoClase
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public virtual ICollection<BloqueHorarioMaterial> BloqueHorarioMaterials { get; set; } = new List<BloqueHorarioMaterial>();
}
