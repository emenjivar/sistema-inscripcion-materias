using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectListItem
using System.ComponentModel.DataAnnotations; // Para validaciones básicas

namespace InscripcionMaterias.Models.ViewModels
{
    public class SimpleInscripcionViewModel
    {
        // --- Propiedades para el Formulario de Inscripción ---
        public int IdInscripcionActual { get; set; } // ID de la inscripción si estamos editando

        [Display(Name = "Ciclo Académico")] // Nombre a mostrar en la vista
        [Required(ErrorMessage = "El Ciclo Académico es requerido.")]
        public int CicloAcademico { get; set; }

        [Display(Name = "Año")]
        [Required(ErrorMessage = "El Año es requerido.")]
        public int Anio { get; set; }

        [Display(Name = "Pensum")]
        [Required(ErrorMessage = "El Pensum es requerido.")]
        public int IdPensumSeleccionado { get; set; } // ID del pensum seleccionado
        public string Estado { get; set; }
        public List<SelectListItem> ListaEstados { get; set; } = new List<SelectListItem>();

        // Lista de Pensums para el dropdown del primer formulario
        public List<SelectListItem> ListaPensums { get; set; } = new List<SelectListItem>();

        // --- Propiedades para el Formulario de Bloque de Clase ---
        [Display(Name = "Materia")]
        [Required(ErrorMessage = "La Materia es requerida.")]
        public int IdMateriaSeleccionada { get; set; }

        [Display(Name = "Grupo")]
        [Required(ErrorMessage = "El Grupo es requerido.")]
        public int IdGrupoSeleccionado { get; set; }

        [Display(Name = "Día de la Semana")]
        [Required(ErrorMessage = "El Día de la Semana es requerido.")]
        public string DiaSemana { get; set; } = null!;

        [Display(Name = "Hora Inicio")]
        [Required(ErrorMessage = "La Hora de Inicio es requerida.")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Formato de hora inválido (HH:MM).")]
        public string HoraInicioString { get; set; } = null!; // Como string para que el usuario escriba

        [Display(Name = "Hora Fin")]
        [Required(ErrorMessage = "La Hora Fin es requerida.")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Formato de hora inválido (HH:MM).")]
        public string HoraFinString { get; set; } = null!; // Como string para que el usuario escriba

        // Listas para los dropdowns del segundo formulario
        public List<SelectListItem> ListaMaterias { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListaGrupos { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListaDiasSemana { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Lunes", Text = "Lunes" },
            new SelectListItem { Value = "Martes", Text = "Martes" },
            new SelectListItem { Value = "Miércoles", Text = "Miércoles" },
            new SelectListItem { Value = "Jueves", Text = "Jueves" },
            new SelectListItem { Value = "Viernes", Text = "Viernes" },
            new SelectListItem { Value = "Sábado", Text = "Sábado" },
            new SelectListItem { Value = "Domingo", Text = "Domingo" }
        };

        // --- Propiedad para la Tabla de Bloques de Clase ---
        // Aquí guardaremos los bloques de clase que se mostrarán en la tabla
        public List<BloqueDeClaseParaTablaViewModel> BloquesEnTabla { get; set; } = new List<BloqueDeClaseParaTablaViewModel>();
    }

    // ViewModel auxiliar para los datos de la tabla de bloques de clase
    public class BloqueDeClaseParaTablaViewModel
    {
        public int Id { get; set; } // ID del bloque en la base de datos (si ya está guardado)
        public string MateriaNombre { get; set; } = null!;
        public string GrupoCodigo { get; set; } = null!;
        public string BloqueCompleto { get; set; } = null!; // Ej: "Lunes 08:00 - 09:50"
    }
}
