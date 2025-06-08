using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InscripcionMaterias.Models;
using InscripcionMaterias.Models.ViewModels;

namespace InscripcionMaterias.Controllers
{
    public class ResultadosCicloAcademicoController : Controller
    {
        private readonly GestionDbContext _context;

        public ResultadosCicloAcademicoController(GestionDbContext context)
        {
            _context = context;
        }

        // GET: ResultadosCicloAcademico
        public async Task<IActionResult> Index()
        {
            var gestionDbContext = _context.ResultadoCicloAcademicos.Include(r => r.IdAlumnoNavigation).Include(r => r.IdMateriaNavigation);
            return View(await gestionDbContext.ToListAsync());
        }

        // GET: ResultadosCicloAcademico/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultadoCicloAcademico = await _context.ResultadoCicloAcademicos
                .Include(r => r.IdAlumnoNavigation)
                .Include(r => r.IdMateriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultadoCicloAcademico == null)
            {
                return NotFound();
            }

            return View(resultadoCicloAcademico);
        }

        // GET: ResultadosCicloAcademico/Create
        public IActionResult Create()
        {
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "Id", "Id");
            ViewData["IdMateria"] = new SelectList(_context.Materia, "Id", "Id");
            return View();
        }

        // POST: ResultadosCicloAcademico/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAlumno,IdMateria,Aprobado")] ResultadoCicloAcademico resultadoCicloAcademico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resultadoCicloAcademico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "Id", "Id", resultadoCicloAcademico.IdAlumno);
            ViewData["IdMateria"] = new SelectList(_context.Materia, "Id", "Id", resultadoCicloAcademico.IdMateria);
            return View(resultadoCicloAcademico);
        }

        // GET: ResultadosCicloAcademico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultadoCicloAcademico = await _context.ResultadoCicloAcademicos.FindAsync(id);
            if (resultadoCicloAcademico == null)
            {
                return NotFound();
            }
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "Id", "Id", resultadoCicloAcademico.IdAlumno);
            ViewData["IdMateria"] = new SelectList(_context.Materia, "Id", "Id", resultadoCicloAcademico.IdMateria);
            return View(resultadoCicloAcademico);
        }

        // POST: ResultadosCicloAcademico/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdAlumno,IdMateria,Aprobado")] ResultadoCicloAcademico resultadoCicloAcademico)
        {
            if (id != resultadoCicloAcademico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resultadoCicloAcademico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultadoCicloAcademicoExists(resultadoCicloAcademico.Id))
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
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "Id", "Id", resultadoCicloAcademico.IdAlumno);
            ViewData["IdMateria"] = new SelectList(_context.Materia, "Id", "Id", resultadoCicloAcademico.IdMateria);
            return View(resultadoCicloAcademico);
        }

        // GET: ResultadosCicloAcademico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultadoCicloAcademico = await _context.ResultadoCicloAcademicos
                .Include(r => r.IdAlumnoNavigation)
                .Include(r => r.IdMateriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultadoCicloAcademico == null)
            {
                return NotFound();
            }

            return View(resultadoCicloAcademico);
        }

        // POST: ResultadosCicloAcademico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resultadoCicloAcademico = await _context.ResultadoCicloAcademicos.FindAsync(id);
            if (resultadoCicloAcademico != null)
            {
                _context.ResultadoCicloAcademicos.Remove(resultadoCicloAcademico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultadoCicloAcademicoExists(int id)
        {
            return _context.ResultadoCicloAcademicos.Any(e => e.Id == id);
        }


        private async Task PoblarSelects(FiltroCierreCicloViewModel model)
        {
            model.Pensums = await _context.Pensums
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Carrera
                }).ToListAsync();

            model.Ciclos = await _context.Inscripcions
                .Select(i => i.CicloAcademico)
                .Distinct()
                .OrderBy(i => i)
                .Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = "Ciclo " + i
                }).ToListAsync();

            model.Anios = await _context.Inscripcions
                .Select(i => i.Anio)
                .Distinct()
                .OrderByDescending(i => i)
                .Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                }).ToListAsync();
        }


        //funcionalidad para cerrar ciclo academico.
        public async Task<IActionResult> CerrarCiclo()
        {
            var model = new FiltroCierreCicloViewModel
            {
                Pensums = await _context.Pensums
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Carrera
                    }).ToListAsync(),

                Ciclos = await _context.Inscripcions
                    .Select(i => i.CicloAcademico)
                    .Distinct()
                    .OrderBy(i => i)
                    .Select(i => new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = "Ciclo " + i
                    }).ToListAsync(),

                Anios = await _context.Inscripcions
                    .Select(i => i.Anio)
                    .Distinct()
                    .OrderByDescending(i => i)
                    .Select(i => new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    }).ToListAsync()
            };

            return View(model);
        }



        //accion post para guardar el cierre de ciclo.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CerrarCiclo(FiltroCierreCicloViewModel model)
        {
            if (model.IdPensum == null || model.CicloAcademico == null || model.Anio == null)
            {
                ModelState.AddModelError("", "Debe seleccionar pensum, ciclo y año.");
                await PoblarSelects(model);
                return View(model);
            }

            var alumnos = await _context.InscripcionAlumnos
                .Where(ia =>
                    ia.IdBloqueHorarioMateriaNavigation.IdInscripcionNavigation.IdPensum == model.IdPensum &&
                    ia.IdBloqueHorarioMateriaNavigation.IdInscripcionNavigation.CicloAcademico == model.CicloAcademico &&
                    ia.IdBloqueHorarioMateriaNavigation.IdInscripcionNavigation.Anio == model.Anio)
                .Select(ia => new
                {
                    Alumno = ia.IdAlumnoNavigation,
                    Materia = ia.IdBloqueHorarioMateriaNavigation.IdMateriaNavigation
                })
                .GroupBy(x => x.Alumno)
                .Select(g => new CierreCicloViewModel
                {
                    IdAlumno = g.Key.Id,
                    NombreAlumno = g.Key.Nombres + " " + g.Key.Apellidos,
                    Materias = g.Select(m => new MateriaCierreVM
                    {
                        IdMateria = m.Materia.Id,
                        NombreMateria = m.Materia.Nombre,
                        Aprobado = false // Puedes ajustarlo si ya hay información previa
                    }).ToList()
                })
                .ToListAsync();

            model.Resultados = alumnos;
            await PoblarSelects(model);
            return View(model);
        }



    }
}
