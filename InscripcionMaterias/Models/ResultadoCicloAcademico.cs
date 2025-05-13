using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;

public partial class ResultadoCicloAcademico
{
    public int Id { get; set; }

    public int IdAlumno { get; set; }

    public int IdMateria { get; set; }

    public bool Aprobado { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    public virtual Materium IdMateriaNavigation { get; set; } = null!;
}
