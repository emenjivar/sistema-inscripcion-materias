using System;
using System.Collections.Generic;

namespace InscripcionMaterias.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Nombre { get; set; }

    public string Password { get; set; } = null!;

    public string Rol { get; set; } = null!;
}
