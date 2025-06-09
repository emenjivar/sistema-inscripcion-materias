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
    public class AlumnosController : Controller
    {
        private readonly GestionDbContext _context;

        public AlumnosController(GestionDbContext context)
        {
            _context = context;
        }

        private void CargarPensumsActivos()
        {
            var pensumsActivos = _context.Pensums
                .Where(p => p.Estado == true)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Carrera
                }).ToList();

            ViewBag.Pensums = pensumsActivos;
        }

        public async Task<IActionResult> Index()
        {
            var alumnos = await _context.Alumnos
                .Include(a => a.IdPensumNavigation)
                .ToListAsync();
            return View(alumnos);
        }

        // POST: Alumnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Carnet,Nombres,Apellidos,IdPensum,IdUsuario")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        public async Task<IActionResult> AlumnosTabla()
        {
            var alumnos = await _context.Alumnos
                .Include(a => a.IdPensumNavigation)
                .ToListAsync();
            return PartialView("_AlumnosTabla", alumnos);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();

            CargarPensumsActivos();
            return PartialView("Edit", alumno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Alumno alumno)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos." });

            try
            {
                _context.Update(alumno);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
                return Json(new { success = false, message = "Alumno no encontrado." });

            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
