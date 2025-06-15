using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using InscripcionMaterias.Models;
using InscripcionMaterias.Models.ViewModels;

namespace InscripcionMaterias.Controllers
{
    public class InscripcionController : Controller
    {
        private readonly GestionDbContext _context;

        public InscripcionController(GestionDbContext context)
        {
            _context = context;
        }




        // ACCIÓN: Para mostrar la lista de inscripciones
        public async Task<IActionResult> ListaInscripciones()
        {
            var viewModel = new ListaInscripcionesViewModel();

            viewModel.Inscripciones = await _context.Inscripcions
                .Include(i => i.IdPensumNavigation) //  cargar el Pensum para obtener el nombre de la carrera
                .Select(i => new InscripcionEnListaViewModel
                {
                    Id = i.Id,
                    CicloAcademico = i.CicloAcademico,
                    Anio = i.Anio,
                    CarreraPensum = i.IdPensumNavigation != null ? i.IdPensumNavigation.Carrera : "N/A", // Obtie el nombre de la carrera
                    Estado = i.Estado,
                    
                })
                .OrderByDescending(i => i.Anio)
                .ThenByDescending(i => i.CicloAcademico)
                .ToListAsync();

            return View(viewModel); // Retorna la vista ListaInscripciones.cshtml
        }




        // GET: Inscripcion/Index (Esta acción cargará el formulario AperturaInscripciones.cshtml)
        // La vista AperturaInscripciones.cshtml es la que realmente mostrará el formulario
        // y necesitará el SimpleInscripcionViewModel.
        public async Task<IActionResult> Index(int? idInscripcion) //se us idInscripcion para editar
        {
            var viewModel = new SimpleInscripcionViewModel();
            await CargarDropdownsParaViewModel(viewModel); //carga los estados fijos

           
            
            
            // --- Cargar datos para los Dropdowns desde la base de datos ---
            // Esto se carga SIEMPRE que se acceda a este formulario
            viewModel.ListaPensums = await _context.Pensums
                                            .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Carrera })
                                            .ToListAsync();

            viewModel.ListaMaterias = await _context.Materia
                                            .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre })
                                            .ToListAsync();

            viewModel.ListaGrupos = await _context.GrupoClases
                                            .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Codigo })
                                            .ToListAsync();



            // --- Lógica para cargar una inscripción existente si idInscripcion es proporcionado ---
            if (idInscripcion.HasValue && idInscripcion.Value > 0)
            {
                var inscripcionExistente = await _context.Inscripcions
                                                          .FirstOrDefaultAsync(i => i.Id == idInscripcion.Value);

                if (inscripcionExistente != null)
                {
                    viewModel.IdInscripcionActual = inscripcionExistente.Id;
                    viewModel.CicloAcademico = inscripcionExistente.CicloAcademico;
                    viewModel.Anio = inscripcionExistente.Anio;
                    viewModel.IdPensumSeleccionado = inscripcionExistente.IdPensum;
                    viewModel.Estado = inscripcionExistente.Estado;

                    await CargarBloquesEnTabla(viewModel, inscripcionExistente.Id);
                }
                else
                {
                    TempData["ErrorMessage"] = "Inscripción no encontrada para editar.";
                    // Si no se encuentra, inicializa para una nueva inscripción
                    viewModel.Anio = DateTime.Now.Year;
                    viewModel.CicloAcademico = 1;
                    viewModel.Estado = "Consulta";

                }
            }
            else
            {
                // Valores por defecto para una nueva inscripción
                viewModel.Anio = DateTime.Now.Year;
                viewModel.CicloAcademico = 1;
                viewModel.Estado = "Consulta";
            }

            // Aqui se indica que la vista a renderizar es Views/Home/AperturaInscripciones.cshtml
            
            return View("~/Views/Home/AperturaInscripciones.cshtml", viewModel);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarInscripcion(SimpleInscripcionViewModel model)
        {
            // Remover validaciones no necesarias para esta parte
            ModelState.Remove(nameof(model.IdMateriaSeleccionada));
            ModelState.Remove(nameof(model.IdGrupoSeleccionado));
            ModelState.Remove(nameof(model.DiaSemana));
            ModelState.Remove(nameof(model.HoraInicioString));
            ModelState.Remove(nameof(model.HoraFinString));

            if (ModelState.IsValid)
            {
                await CargarDropdownsParaViewModel(model);//cargar tambien dropdown de estaod
                Inscripcion inscripcion;
                if (model.IdInscripcionActual > 0)
                {
                    
                    inscripcion = await _context.Inscripcions.FindAsync(model.IdInscripcionActual);
                    if (inscripcion == null)
                    {
                        TempData["ErrorMessage"] = "Inscripción no encontrada para actualizar.";
                        // Recargar dropdowns y devolver vista con error
                        await CargarDropdownsParaViewModel(model);
                        return View("~/Views/Home/AperturaInscripciones.cshtml", model);
                    }
                    inscripcion.CicloAcademico = model.CicloAcademico;
                    inscripcion.Anio = model.Anio;
                    inscripcion.IdPensum = model.IdPensumSeleccionado;
                    inscripcion.Estado = model.Estado;
                    _context.Update(inscripcion);
                }
                else
                {
                    inscripcion = new Inscripcion
                    {
                        CicloAcademico = model.CicloAcademico,
                        Anio = model.Anio,
                        IdPensum = model.IdPensumSeleccionado,
                        Estado = model.Estado
                    };
                    _context.Add(inscripcion);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Datos de inscripción guardados correctamente.";
                // Redirige a la misma acción Index de InscripcionController, que renderizará el formulario con los datos actualizados
                return RedirectToAction(nameof(ListaInscripciones));
            }

            // Si hay errores de validación, recargar los dropdowns y bloques antes de volver a la vista
            await CargarDropdownsParaViewModel(model);
            if (model.IdInscripcionActual > 0)
            {
                await CargarBloquesEnTabla(model, model.IdInscripcionActual);
            }
            TempData["ErrorMessage"] = "Error al guardar los datos de inscripción. Revise los campos.";
            return View("~/Views/Home/AperturaInscripciones.cshtml", model); // Vuelve a la vista del formulario con errores
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarBloque(SimpleInscripcionViewModel model)
        {
            // Remover validaciones no necesarias para esta parte
            ModelState.Remove(nameof(model.CicloAcademico));
            ModelState.Remove(nameof(model.Anio));
            ModelState.Remove(nameof(model.IdPensumSeleccionado));
            ModelState.Remove(nameof(model.Estado));




            if (model.IdInscripcionActual <= 0)
            {
                ModelState.AddModelError("", "Primero debe guardar la información de la inscripción para poder agregar bloques.");
            }

            TimeOnly horaInicio;
            TimeOnly horaFin;
            bool horaInicioValida = TimeOnly.TryParse(model.HoraInicioString, out horaInicio);
            bool horaFinValida = TimeOnly.TryParse(model.HoraFinString, out horaFin);

            if (!horaInicioValida)
            {
                ModelState.AddModelError(nameof(model.HoraInicioString), "El formato de la hora de inicio es inválido (HH:MM).");
            }
            if (!horaFinValida)
            {
                ModelState.AddModelError(nameof(model.HoraFinString), "El formato de la hora fin es inválido (HH:MM).");
            }

            if (ModelState.IsValid)
            {
                await CargarDropdownsParaViewModel(model); // Recarga todos los dropdowns, incluyendo Estados
                var bloque = new BloqueHorarioMaterial
                {
                    IdInscripcion = model.IdInscripcionActual,
                    IdMateria = model.IdMateriaSeleccionada,
                    IdGrupo = model.IdGrupoSeleccionado,
                    DiaSemana = model.DiaSemana, 
                    HoraInicio = horaInicio,
                    HoraFin = horaFin
                };

                _context.Add(bloque);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Bloque de clase agregado correctamente.";

                await CargarBloquesEnTabla(model, model.IdInscripcionActual);

                // Devuelve la vista parcial solo para la tabla
                return PartialView("_BloquesDeClaseTabla", model);
            }
            else
            {
                // Si hay errores en el segundo formulario, recargar dropdowns y tabla
                await CargarDropdownsParaViewModel(model);
                if (model.IdInscripcionActual > 0)
                {
                    await CargarBloquesEnTabla(model, model.IdInscripcionActual);
                }
                TempData["ErrorMessage"] = "Error al agregar el bloque de clase. Revise los campos.";
                return PartialView("_BloquesDeClaseTabla", model); 
            }
        }

 
        private async Task CargarBloquesEnTabla(SimpleInscripcionViewModel viewModel, int idInscripcion)
        {
            viewModel.BloquesEnTabla.Clear();
            var bloquesDb = await _context.BloqueHorarioMaterials
                                        .Where(b => b.IdInscripcion == idInscripcion)
                                        .Include(b => b.IdMateriaNavigation)
                                        .Include(b => b.IdGrupoNavigation)
                                        .ToListAsync();
            foreach (var bloque in bloquesDb)
            {
                viewModel.BloquesEnTabla.Add(new BloqueDeClaseParaTablaViewModel
                {
                    Id = bloque.Id,
                    MateriaNombre = bloque.IdMateriaNavigation?.Nombre ?? "N/A",
                    GrupoCodigo = bloque.IdGrupoNavigation?.Codigo ?? "N/A",
                    BloqueCompleto = $"{bloque.DiaSemana} {bloque.HoraInicio.ToString("hh\\:mm")} - {bloque.HoraFin.ToString("hh\\:mm")}"
                });
            }
        }

        // Helper para recargar dropdowns
        private async Task CargarDropdownsParaViewModel(SimpleInscripcionViewModel viewModel)
        {
            viewModel.ListaPensums = await _context.Pensums
                                            .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Carrera })
                                            .ToListAsync();
            viewModel.ListaMaterias = await _context.Materia
                                            .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre })
                                            .ToListAsync();
            viewModel.ListaGrupos = await _context.GrupoClases
                                            .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Codigo })
                                            .ToListAsync();
            //Cargar Estados con valores fijos
            viewModel.ListaEstados = new List<SelectListItem>
            {
                new SelectListItem { Value = "Consulta", Text = "Consulta" },
                new SelectListItem { Value = "Inscripcion", Text = "Inscripción" }, 
                new SelectListItem { Value = "Cerrado", Text = "Cerrado" }
            };

            //Cargar Días de la Semana
            viewModel.ListaDiasSemana = new List<SelectListItem>
            {
                new SelectListItem { Value = "Lunes", Text = "Lunes" },
                new SelectListItem { Value = "Martes", Text = "Martes" },
                new SelectListItem { Value = "Miércoles", Text = "Miércoles" },
                new SelectListItem { Value = "Jueves", Text = "Jueves" },
                new SelectListItem { Value = "Viernes", Text = "Viernes" },
                new SelectListItem { Value = "Sábado", Text = "Sábado" }
            };

        }



        [HttpGet]
        public async Task<JsonResult> GetGruposAsSelectList()
        {
            var grupos = await _context.GrupoClases
                                       .OrderBy(g => g.Codigo)
                                       .Select(g => new SelectListItem
                                       {
                                           Value = g.Id.ToString(),
                                           Text = g.Codigo
                                       })
                                       .ToListAsync();
            return Json(grupos);
        }





    }
}