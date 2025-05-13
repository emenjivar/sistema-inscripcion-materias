using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;

public partial class InscripcionAlumno
{
    public int Id { get; set; }

    public int IdAlumno { get; set; }

    public int IdBloqueHorarioMateria { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    public virtual BloqueHorarioMaterial IdBloqueHorarioMateriaNavigation { get; set; } = null!;
}
