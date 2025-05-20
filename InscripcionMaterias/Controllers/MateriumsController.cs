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
    public class MateriumsController : Controller
    {
        private readonly GestionDbContext _context;

        public MateriumsController(GestionDbContext context)
        {
            _context = context;
        }

        // GET: Materiums
        public async Task<IActionResult> Index()
        {
            return View(await _context.Materia.ToListAsync());
        }

        public async Task<IActionResult> MateriasTabla()
        {
            var materias = await _context.Materia.ToListAsync();
            return PartialView("_MateriasTabla", materias);
        }

        // GET: Materiums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materium = await _context.Materia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materium == null)
            {
                return NotFound();
            }

            return View(materium);
        }

        // GET: Materiums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materiums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Nombre,UnidadesValorativas,Descripcion")] Materium materium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materium);
        }

        // GET: Materiums/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var materia = await _context.Materia.FindAsync(id);
            if (materia == null)
                return NotFound();

            return PartialView("Edit", materia);
        }


        // POST: Materiums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Materium materium)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos." });

            try
            {
                _context.Update(materium);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // POST: Materiums/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var materium = await _context.Materia.FindAsync(id);
            if (materium == null)
            {
                return Json(new { success = false, message = "Materia no encontrada." });
            }

            _context.Materia.Remove(materium);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }


        private bool MateriumExists(int id)
        {
            return _context.Materia.Any(e => e.Id == id);
        }
    }
}
