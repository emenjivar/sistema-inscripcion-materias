using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InscripcionMaterias.Models;
public partial class Alumno
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El carnet es obligatorio.")]
    [StringLength(20, ErrorMessage = "El carnet no puede tener más de 20 caracteres.")]
    public string Carnet { get; set; } = null!;

    [Display(Name = "Pensum")]
    public int? IdPensum { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public virtual Pensum? IdPensumNavigation { get; set; }

    public virtual ICollection<InscripcionAlumno> InscripcionAlumnos { get; set; } = new List<InscripcionAlumno>();

    public virtual ICollection<ResultadoCicloAcademico> ResultadoCicloAcademicos { get; set; } = new List<ResultadoCicloAcademico>();
}
