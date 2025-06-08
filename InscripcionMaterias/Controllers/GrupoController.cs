using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using InscripcionMaterias.Models; 
using InscripcionMaterias.Models.ViewModels; 
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic; 

namespace InscripcionMaterias.Controllers
{
    public class GrupoController : Controller
    {
        private readonly GestionDbContext _context;

        public GrupoController(GestionDbContext context)
        {
            _context = context;
        }

        // GET: Grupo/Index (Lista de Grupos)
        public async Task<IActionResult> Index()
        {
            var grupos = await _context.GrupoClases
                                       .OrderBy(g => g.Codigo)
                                       .ToListAsync();
            return View(grupos);
        }

        // GET: Grupo/Crear
        public IActionResult Crear()
        {
            return View("FormularioGrupo", new GrupoClaseViewModel()); // Usaremos una vista compartida
        }

        // GET: Grupo/Editar/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupo = await _context.GrupoClases.FindAsync(id);
            if (grupo == null)
            {
                return NotFound();
            }

            // Mapear la entidad al ViewModel para edición (solo Id y Codigo)
            var viewModel = new GrupoClaseViewModel
            {
                Id = grupo.Id,
                Codigo = grupo.Codigo
            };

            return View("FormularioGrupo", viewModel); // Usaremos la misma vista para crear y editar
        }

        // POST: Grupo/Guardar (para Crear y Editar)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(GrupoClaseViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0) // Es un nuevo grupo (crear)
                {
                    var grupo = new GrupoClase
                    {
                        Codigo = model.Codigo
                    };
                    _context.Add(grupo);
                    TempData["SuccessMessage"] = "Grupo creado exitosamente.";
                }
                else // Es un grupo existente (editar)
                {
                    var grupo = await _context.GrupoClases.FindAsync(model.Id);
                    if (grupo == null)
                    {
                        TempData["ErrorMessage"] = "Grupo no encontrado para actualizar.";
                        return View("FormularioGrupo", model);
                    }

                    grupo.Codigo = model.Codigo;
                    _context.Update(grupo);
                    TempData["SuccessMessage"] = "Grupo actualizado exitosamente.";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirige a la lista de grupos
            }

            // Si hay errores de validación, vuelve a la vista del formulario
            TempData["ErrorMessage"] = "Error al guardar el grupo. Revise los campos.";
            return View("FormularioGrupo", model);
        }

        // POST: Grupo/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var grupo = await _context.GrupoClases.FindAsync(id);
            if (grupo == null)
            {
                TempData["ErrorMessage"] = "Grupo no encontrado para eliminar.";
                return RedirectToAction(nameof(Index));
            }

            // Advertencia: Considera si el grupo está asociado a inscripciones o bloques antes de eliminarlo.
            // Si está asociado, esto podría causar un error de restricción de clave externa.
            // Es buena práctica verificar o manejar esta excepción, o implementar eliminación en cascada si es apropiado.

            _context.GrupoClases.Remove(grupo); // Corrección: Usar _context.GrupoClases
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Grupo eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }


        //GET para cargar el formulario de creación de grupo en un modal
        [HttpGet]
        public IActionResult _CrearGrupoModal()
        {
            // Devuelve la vista parcial del formulario con un ViewModel vacío para la creación
            return PartialView("FormularioGrupo", new GrupoClaseViewModel());
        }

        // POST para guardar el grupo desde el modal (vía AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _GuardarGrupoModal(GrupoClaseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var grupo = new GrupoClase
                {
                    Codigo = model.Codigo
                    // Nombre y Capacidad no se incluyen según tu decisión actual
                };

                // Verificar si el código ya existe
                if (await _context.GrupoClases.AnyAsync(g => g.Codigo == model.Codigo))
                {
                    ModelState.AddModelError("Codigo", "Ya existe un grupo con este código.");
                    // Si hay un error de validación personalizado, devolver la vista parcial para mostrarlo
                    return PartialView("FormularioGrupo", model);
                }

                _context.Add(grupo);
                await _context.SaveChangesAsync();

                // Devuelve un JSON indicando éxito y el ID del nuevo grupo
                return Json(new { success = true, message = "Grupo creado exitosamente.", newGroupId = grupo.Id, newGroupText = grupo.Codigo });
            }

            // Si hay errores de validación, devuelve la vista parcial con los errores
            return PartialView("FormularioGrupo", model);
        }








    }
}