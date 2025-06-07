﻿using System.ComponentModel.DataAnnotations;

namespace InscripcionMaterias.CustomModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [Display(Name = "Usuario/Carnet")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = null!;
    }
}
