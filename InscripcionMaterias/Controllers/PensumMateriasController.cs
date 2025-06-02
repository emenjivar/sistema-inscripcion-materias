using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InscripcionMaterias.Models;
using System.Drawing;

namespace InscripcionMaterias.Controllers
{
    public class PensumMateriasController : Controller
    {
        private readonly GestionDbContext _context;

        public PensumMateriasController(GestionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ConfigurarMaterias(int id, int cantidadCiclos)
        {
            var pensum = await _context.Pensums.FirstOrDefaultAsync(p => p.Id == id);
            if (pensum == null) return NotFound();
            ViewBag.CantidadCiclos = cantidadCiclos;
            ViewBag.NombreCarrera = pensum.Carrera;
            // Obtener todas las materias asignadas a ese pensum (carrera)
            var materiasAsignadas = await _context.PensumMaterias
    .Include(pm => pm.IdMateriaNavigation)  // <-- Aquí la propiedad de navegación
    .Where(pm => pm.IdPensum == id)
    .ToListAsync();


            if (materiasAsignadas == null || materiasAsignadas.Count == 0)
            {
                // Puede que no haya materias asignadas todavía, no es error
                materiasAsignadas = new List<PensumMateria>();
            }

            // Obtener todas las materias disponibles para seleccionar (para agregar al pensum)
            var materiasDisponibles = await _context.Materia.ToListAsync();

            // Pasar materias disponibles por ViewBag
            ViewBag.MateriasDisponibles = materiasDisponibles;

            // Pasar el Id del pensum para usarlo en el formulario
            ViewBag.IdPensum = id;

            // Retornar la vista con las materias asignadas (como modelo)
             return View(materiasAsignadas);
          //  return PartialView("ConfigurarMaterias", materiasAsignadas);

        }


        // GET: PensumMaterias
        public async Task<IActionResult> Index()
        {
            var gestionDbContext = _context.PensumMaterias.Include(p => p.IdMateriaNavigation).Include(p => p.IdMateriaPrerequisitoNavigation).Include(p => p.IdPensumNavigation);
            return View(await gestionDbContext.ToListAsync());
        }

        // GET: PensumMaterias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pensumMateria = await _context.PensumMaterias
                .Include(p => p.IdMateriaNavigation)
                .Include(p => p.IdMateriaPrerequisitoNavigation)
                .Include(p => p.IdPensumNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pensumMateria == null)
            {
                return NotFound();
            }

            return View(pensumMateria);
        }

        // GET: PensumMaterias/Create
     /*   public IActionResult Create()
        {
            ViewData["IdMateria"] = new SelectList(_context.Materia, "Id", "Id");
            ViewData["IdMateriaPrerequisito"] = new SelectList(_context.Materia, "Id", "Id");
            ViewData["IdPensum"] = new SelectList(_context.Pensums, "Id", "Id");
            return View();
        }*/

        // POST: PensumMaterias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPensum,IdMateria,IdMateriaPrerequisito,CicloCurricular")] PensumMateria pensumMateria)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new {
                        Campo = x.Key,
                        Errores = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    });

                return Json(new { success = false, message = "Datos inválidos.", detalles = errores });
            }


            try
            {
                _context.PensumMaterias.Add(pensumMateria);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Materia guardada con éxito" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al guardar: " + ex.Message });
            }

        }

        public IActionResult ListarMateriasAsignadas(int idPensum)
        {
            var materias = _context.PensumMaterias
                .Where(pm => pm.IdPensum == idPensum)
                .Include(pm => pm.IdMateriaNavigation)
                .ToList();

            var cantidadCiclos = materias.Any() ? materias.Max(m => m.CicloCurricular) : 0;
            // o como lo calcules normalmente
            var nombreCarrera = _context.Pensums
                .Where(p => p.Id == idPensum)
                .Select(p => p.Carrera)
                .FirstOrDefault() ?? "Carrera no encontrada";

            ViewBag.CantidadCiclos = cantidadCiclos;
            ViewBag.NombreCarrera = nombreCarrera;

            return PartialView("_MateriasAsignadasPartial", materias);
        }



        // GET: PensumMaterias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pensumMateria = await _context.PensumMaterias.FindAsync(id);
            if (pensumMateria == null)
            {
                return NotFound();
            }
            ViewData["IdMateria"] = new SelectList(_context.Materia, "Id", "Id", pensumMateria.IdMateria);
            ViewData["IdMateriaPrerequisito"] = new SelectList(_context.Materia, "Id", "Id", pensumMateria.IdMateriaPrerequisito);
            ViewData["IdPensum"] = new SelectList(_context.Pensums, "Id", "Id", pensumMateria.IdPensum);
            return View(pensumMateria);
        }

        // POST: PensumMaterias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPensum,IdMateria,IdMateriaPrerequisito,CicloCurricular")] PensumMateria pensumMateria)
        {
            if (id != pensumMateria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pensumMateria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PensumMateriaExists(pensumMateria.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMateria"] = new SelectList(_context.Materia, "Id", "Id", pensumMateria.IdMateria);
            ViewData["IdMateriaPrerequisito"] = new SelectList(_context.Materia, "Id", "Id", pensumMateria.IdMateriaPrerequisito);
            ViewData["IdPensum"] = new SelectList(_context.Pensums, "Id", "Id", pensumMateria.IdPensum);
            return View(pensumMateria);
        }

        // POST: PensumMaterias/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var materiaPensum = await _context.PensumMaterias.FindAsync(id);
            if (materiaPensum == null)
            {
                return Json(new { success = false, message = "No se encontró la relación materia-pensum." });
            }

            _context.PensumMaterias.Remove(materiaPensum);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        private bool PensumMateriaExists(int id)
        {
            return _context.PensumMaterias.Any(e => e.Id == id);
        }

        public async Task<IActionResult> MiPensum(string? idEstudiante = "061818")
        {
            if (idEstudiante == null)
                return BadRequest("No se proporcionó el ID del estudiante");

            var estudiante = await _context.Alumnos
                .Include(e => e.IdPensumNavigation)
                .FirstOrDefaultAsync(e => e.Carnet == idEstudiante);

            if (estudiante == null)
                return NotFound("Estudiante no encontrado");

            if (estudiante.IdPensum == null)
                return BadRequest("El estudiante no tiene un pensum asignado");

            var materiasPensum = await _context.PensumMaterias
                .Include(pm => pm.IdMateriaNavigation)
                .Where(pm => pm.IdPensum == estudiante.IdPensum)
                .ToListAsync();

            ViewBag.NombreCarrera = estudiante.IdPensumNavigation.Carrera;

            // Calcular el total de ciclos y años
            int maxCiclo = materiasPensum.Max(m => m.CicloCurricular);
            int cantidadAnios = (int)Math.Ceiling(maxCiclo / 2.0);

            ViewBag.CantidadCiclos = maxCiclo;
            ViewBag.CantidadAnios = cantidadAnios;

            return View("Details", materiasPensum);
        }




    }

}
