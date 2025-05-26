using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InscripcionMaterias.Models;

namespace InscripcionMaterias.Controllers
{
    public class PensumsController : Controller
    {
        private readonly GestionDbContext _context;

        public PensumsController(GestionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PensumTabla()
        {
            var lista = await _context.Pensums
                //.Where(p => p.Estado)
                .ToListAsync();
            return PartialView("_PensumTabla", lista);
        }


        // GET: Pensums
        public async Task<IActionResult> Index()
        {
            var pensumsActivos = await _context.Pensums
                //.Where(p => p.Estado)
                .ToListAsync();
            return View(pensumsActivos);
        }


        // GET: Pensums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pensum = await _context.Pensums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pensum == null)
            {
                return NotFound();
            }

            return View(pensum);
        }

        // POST: Pensums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Carrera,TipoCarrera,CantidadCiclos")] Pensum pensum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pensum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(pensum); // Si hay error, vuelve a mostrar el formulario
        }


        // GET: Pensums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pensum = await _context.Pensums.FindAsync(id);
            if (pensum == null)
            {
                return NotFound();
            }
            return PartialView("Edit", pensum);
        }

        // POST: Pensums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Carrera,TipoCarrera")] Pensum pensum)
        {
            if (id != pensum.Id)
                return Json(new { success = false, message = "ID inválido" });

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pensum);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Pensums.Any(e => e.Id == pensum.Id))
                        return Json(new { success = false, message = "Pensum no encontrado" });

                    return Json(new { success = false, message = "Error de concurrencia" });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = string.Join("; ", errors) });
        }



        // POST: Pensums/Delete/5
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var pensum = await _context.Pensums.FindAsync(id);
            if (pensum == null)
            {
                return Json(new { success = false, message = "Pensum no encontrado." });
            }

            pensum.Estado = !pensum.Estado; // ✅ Cambia entre true y false
            _context.Pensums.Update(pensum);
            await _context.SaveChangesAsync();

            return Json(new { success = true, estado = pensum.Estado });
        }

        private bool PensumExists(int id)
        {
            return _context.Pensums.Any(e => e.Id == id);
        }


        public IActionResult ConfigurarMaterias(int id)
        {
            // Redirige hacia el controlador PensumMaterias, acción Index o Configurar, con parámetro id
            return RedirectToAction("_ConfigurarMaterias", "PensumMaterias", new { id = id });
        }
    }
}
