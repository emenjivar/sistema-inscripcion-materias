using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;

public partial class Alumno
{
    public int Id { get; set; }

    public string Carnet { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public int? IdPensum { get; set; }

    public int IdUsuario { get; set; }

    public virtual Pensum? IdPensumNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<InscripcionAlumno> InscripcionAlumnos { get; set; } = new List<InscripcionAlumno>();

    public virtual ICollection<ResultadoCicloAcademico> ResultadoCicloAcademicos { get; set; } = new List<ResultadoCicloAcademico>();
}
