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

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, ErrorMessage = "El nombre no puede tener más de 60 caracteres.")]
    public string Nombres { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(60, ErrorMessage = "El apellido no puede tener más de 60 caracteres.")]
    public string Apellidos { get; set; } = null!;

    [Display(Name = "Pensum")]
    public int? IdPensum { get; set; }

    [Required(ErrorMessage = "El usuario asociado es obligatorio.")]
    public int IdUsuario { get; set; }


    //Las siguientes 3 lineas se agregan para evitar errores de referencia en PneusmMateriasController

    public virtual Pensum? IdPensumNavigation { get; set; }
    public virtual ICollection<InscripcionAlumno> InscripcionAlumnos { get; set; } = new List<InscripcionAlumno>();
    public virtual ICollection<ResultadoCicloAcademico> ResultadoCicloAcademicos { get; set; } = new List<ResultadoCicloAcademico>();

}
