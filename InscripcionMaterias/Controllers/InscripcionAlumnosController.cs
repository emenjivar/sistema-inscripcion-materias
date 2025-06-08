using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InscripcionMaterias.Models;
using InscripcionMaterias.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace InscripcionMaterias.Controllers
{
    public class InscripcionAlumnosController : Controller
    {
        private readonly GestionDbContext _context;

        public InscripcionAlumnosController(GestionDbContext context)
        {
            _context = context;
        }

        // GET: InscripcionAlumnos
        public async Task<IActionResult> Index()
        {
            var inscripciones = _context.Inscripcions
                .Include(i => i.IdPensumNavigation);
            return View(await inscripciones.ToListAsync());
        }


        // GET: InscripcionAlumnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcionAlumno = await _context.InscripcionAlumnos
                .Include(i => i.IdAlumnoNavigation)
                .Include(i => i.IdBloqueHorarioMateriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inscripcionAlumno == null)
            {
                return NotFound();
            }

            return View(inscripcionAlumno);
        }

        // GET: InscripcionAlumnos/Create
        public IActionResult Create()
        {
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "Id", "Id");
            ViewData["IdBloqueHorarioMateria"] = new SelectList(_context.BloqueHorarioMaterials, "Id", "Id");
            return View();
        }

        // POST: InscripcionAlumnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAlumno,IdBloqueHorarioMateria")] InscripcionAlumno inscripcionAlumno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inscripcionAlumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "Id", "Id", inscripcionAlumno.IdAlumno);
            ViewData["IdBloqueHorarioMateria"] = new SelectList(_context.BloqueHorarioMaterials, "Id", "Id", inscripcionAlumno.IdBloqueHorarioMateria);
            return View(inscripcionAlumno);
        }

        // GET: InscripcionAlumnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcionAlumno = await _context.InscripcionAlumnos.FindAsync(id);
            if (inscripcionAlumno == null)
            {
                return NotFound();
            }
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "Id", "Id", inscripcionAlumno.IdAlumno);
            ViewData["IdBloqueHorarioMateria"] = new SelectList(_context.BloqueHorarioMaterials, "Id", "Id", inscripcionAlumno.IdBloqueHorarioMateria);
            return View(inscripcionAlumno);
        }

        // POST: InscripcionAlumnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdAlumno,IdBloqueHorarioMateria")] InscripcionAlumno inscripcionAlumno)
        {
            if (id != inscripcionAlumno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscripcionAlumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscripcionAlumnoExists(inscripcionAlumno.Id))
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
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "Id", "Id", inscripcionAlumno.IdAlumno);
            ViewData["IdBloqueHorarioMateria"] = new SelectList(_context.BloqueHorarioMaterials, "Id", "Id", inscripcionAlumno.IdBloqueHorarioMateria);
            return View(inscripcionAlumno);
        }

        // GET: InscripcionAlumnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcionAlumno = await _context.InscripcionAlumnos
                .Include(i => i.IdAlumnoNavigation)
                .Include(i => i.IdBloqueHorarioMateriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inscripcionAlumno == null)
            {
                return NotFound();
            }

            return View(inscripcionAlumno);
        }

        // POST: InscripcionAlumnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inscripcionAlumno = await _context.InscripcionAlumnos.FindAsync(id);
            if (inscripcionAlumno != null)
            {
                _context.InscripcionAlumnos.Remove(inscripcionAlumno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscripcionAlumnoExists(int id)
        {
            return _context.InscripcionAlumnos.Any(e => e.Id == id);
        }

        public IActionResult Inscripcion()
        {
            // Validar que el usuario es alumno
            string? rol = HttpContext.Session.GetString("Rol");
            if (rol?.ToLower() != "alumno")
            {
                return Unauthorized(); // También puedes redirigir si lo prefieres
            }

            int? idAlumno = HttpContext.Session.GetInt32("IdAlumno");
            if (idAlumno == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Incluye la navegación al usuario para poder acceder a Username
            var alumno = _context.Alumnos
                .Include(a => a.IdUsuarioNavigation)
                .FirstOrDefault(a => a.Id == idAlumno.Value);

            if (alumno == null)
            {
                return NotFound("Alumno no encontrado.");
            }

            // Ahora sí puedes acceder a Username
            if (alumno.IdUsuarioNavigation != null)
            {
                HttpContext.Session.SetString("Username", alumno.IdUsuarioNavigation.Username);
                HttpContext.Session.SetString("Rol", "alumno");
            }

            var model = new InscripcionViewModel
            {
                NombreAlumno = alumno.Nombres,
                Carnet = alumno.Carnet,
                CicloAcademico = 1, // Esto probablemente deba venir de una tabla o lógica
                Anio = 2025,
                MateriasDisponibles = _context.Materia.ToList(),
                GruposDisponibles = _context.GrupoClases.ToList(),
                MateriasSeleccionadas = new List<InscripcionSeleccionadaViewModel>()
            };

            return View(model);
        }



    }
}
