using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;
public partial class Alumno
{
    public int Id { get; set; }

    public string Carnet { get; set; } = null!;

    public int? IdPensum { get; set; }

    public string Password { get; set; } = null!;

    public virtual Pensum? IdPensumNavigation { get; set; }

    public virtual ICollection<InscripcionAlumno> InscripcionAlumnos { get; set; } = new List<InscripcionAlumno>();

    public virtual ICollection<ResultadoCicloAcademico> ResultadoCicloAcademicos { get; set; } = new List<ResultadoCicloAcademico>();
}
