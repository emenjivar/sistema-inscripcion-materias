using System.Collections.Generic;
using System;

namespace InscripcionMaterias.Models.ViewModels
{
    public class InscripcionEnListaViewModel
    {
        public int Id { get; set; }
        public int CicloAcademico { get; set; }
        public int Anio { get; set; }
        public string CarreraPensum { get; set; } // Para mostrar el nombre del pensum/carrera
        public string Estado { get; set; }
       // public DateTime FechaCreacion { get; set; } // Asumiendo que tienes una fecha de creación
    }

    public class ListaInscripcionesViewModel
    {
        public List<InscripcionEnListaViewModel> Inscripciones { get; set; } = new List<InscripcionEnListaViewModel>();
    }
}