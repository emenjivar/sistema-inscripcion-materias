﻿using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;

public partial class Pensum
{
    public int Id { get; set; }

    public string Carrera { get; set; } = null!;

    public virtual ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();

    public virtual ICollection<PensumMateria> PensumMateria { get; set; } = new List<PensumMateria>();
}
