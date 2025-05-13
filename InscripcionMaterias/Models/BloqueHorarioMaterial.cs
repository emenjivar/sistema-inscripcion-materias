using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;

public partial class BloqueHorarioMaterial
{
    public int Id { get; set; }

    public int IdInscripcion { get; set; }

    public int IdMateria { get; set; }

    public int IdGrupo { get; set; }

    public string DiaSemana { get; set; } = null!;

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public virtual GrupoClase IdGrupoNavigation { get; set; } = null!;

    public virtual Inscripcion IdInscripcionNavigation { get; set; } = null!;

    public virtual Materium IdMateriaNavigation { get; set; } = null!;

    public virtual ICollection<InscripcionAlumno> InscripcionAlumnos { get; set; } = new List<InscripcionAlumno>();
}
