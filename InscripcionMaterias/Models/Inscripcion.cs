using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;

public partial class Inscripcion
{
    public int Id { get; set; }

    public int CicloAcademico { get; set; }

    public int Anio { get; set; }

    public int IdPensum { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<BloqueHorarioMaterial> BloqueHorarioMaterials { get; set; } = new List<BloqueHorarioMaterial>();

    public virtual Pensum IdPensumNavigation { get; set; } = null!;
}
