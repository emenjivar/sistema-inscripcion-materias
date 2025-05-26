using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;

public partial class PensumMateria
{
    public int Id { get; set; }

    public int IdPensum { get; set; }

    public int IdMateria { get; set; }

    public int? IdMateriaPrerequisito { get; set; }

    public int CicloCurricular { get; set; }

    public virtual Materium? IdMateriaNavigation { get; set; }

    public virtual Materium? IdMateriaPrerequisitoNavigation { get; set; }

    public virtual Pensum? IdPensumNavigation { get; set; } = null!;
}
