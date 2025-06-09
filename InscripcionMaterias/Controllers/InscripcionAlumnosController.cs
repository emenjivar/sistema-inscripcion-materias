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

            var bloquesHorarios = _context.BloqueHorarioMaterials
            .Include(b => b.IdMateriaNavigation)
            .Include(b => b.IdGrupoNavigation)
            .Select(b => new GrupoMateriaHorarioViewModel
            {
                IdBloqueHorarioMateria = b.Id,
                MateriaNombre = b.IdMateriaNavigation.Nombre,
                IdMateria = b.IdMateriaNavigation.Id,
                GrupoCodigo = b.IdGrupoNavigation.Codigo,
                HorarioDescripcion = $"{b.DiaSemana} {b.HoraInicio} - {b.HoraFin}"
            })
            .ToList();

            var inscripcionesAlumno = _context.InscripcionAlumnos
            .Include(i => i.IdBloqueHorarioMateriaNavigation)
                .ThenInclude(b => b.IdMateriaNavigation)
            .Include(i => i.IdBloqueHorarioMateriaNavigation)
                .ThenInclude(b => b.IdGrupoNavigation)
            .Where(i => i.IdAlumno == alumno.Id)
            .ToList();

            var materiasSeleccionadas = inscripcionesAlumno.Select(i => new InscripcionSeleccionadaViewModel
            {
                MateriaNombre = i.IdBloqueHorarioMateriaNavigation.IdMateriaNavigation.Nombre,
                Grupo = i.IdBloqueHorarioMateriaNavigation.IdGrupoNavigation.Codigo,
                Horarios = new List<string>
                    {
                        $"{i.IdBloqueHorarioMateriaNavigation.DiaSemana} {i.IdBloqueHorarioMateriaNavigation.HoraInicio} - {i.IdBloqueHorarioMateriaNavigation.HoraFin}"
                    },
                IdBloqueHorarioMateria = i.IdBloqueHorarioMateria
            }).ToList();

            //Calculando cuantas materias mas se pueden inscribir
            int selectsDisponibles = 5 - materiasSeleccionadas.Count;
            if (selectsDisponibles < 0)
                selectsDisponibles = 0;

            // Obtener la inscripción activa
            var inscripcionActiva = _context.Inscripcions
                .FirstOrDefault(i => i.Estado.ToLower() == "inscripcion");

            if (inscripcionActiva == null)
            {
                TempData["Error"] = "No hay un proceso de inscripción activo en este momento.";
                return RedirectToAction("Index", "Home");
            }

            var model = new InscripcionViewModel
            {
                NombreAlumno = alumno.Nombres,
                Carnet = alumno.Carnet,
                CicloAcademico = inscripcionActiva.CicloAcademico,
                Anio = inscripcionActiva.Anio,
                MateriasDisponibles = _context.Materia.ToList(),
                GruposDisponibles = _context.GrupoClases.ToList(),
                GrupoMateriaHorarios = bloquesHorarios,
                MateriasSeleccionadas = materiasSeleccionadas,
                SelectsDisponibles = selectsDisponibles,
                IdInscripcion = inscripcionActiva.Id
            };



            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarInscripcion(InscripcionViewModel model)
        {
            int? idAlumno = HttpContext.Session.GetInt32("IdAlumno");
            if (idAlumno == null)
                return RedirectToAction("Login", "Account");

            // Obtener inscripciones actuales del alumno
            var inscripcionesActuales = _context.InscripcionAlumnos
                .Include(i => i.IdBloqueHorarioMateriaNavigation)
                    .ThenInclude(b => b.IdMateriaNavigation)
                .Where(i => i.IdAlumno == idAlumno.Value)
                .ToList();

            if (inscripcionesActuales.Count >= 5)
            {
                TempData["Error"] = "Ya has inscrito el número máximo de materias permitidas.";
                return RedirectToAction(nameof(Inscripcion));
            }

            int disponibles = 5 - inscripcionesActuales.Count;

            var nuevosBloquesIds = model.MateriasSeleccionadas
                .Where(s => s.IdBloqueHorarioMateria != 0)
                .Select(s => s.IdBloqueHorarioMateria)
                .Distinct()
                .ToList();

            // Cargar todos los bloques seleccionados con sus relaciones
            var bloquesSeleccionados = _context.BloqueHorarioMaterials
                .Where(b => nuevosBloquesIds.Contains(b.Id))
                .Include(b => b.IdMateriaNavigation)
                .ToList();

            // Validar si ya está inscrito en alguna de las materias (sin importar grupo)
            foreach (var nuevoBloque in bloquesSeleccionados)
            {
                int idMateria = nuevoBloque.IdMateria;
                if (inscripcionesActuales.Any(i =>
                    i.IdBloqueHorarioMateriaNavigation.IdMateriaNavigation.Id == idMateria))
                {
                    TempData["Error"] = $"Ya estás inscrito en la materia \"{nuevoBloque.IdMateriaNavigation.Nombre}\" en otro grupo.";
                    return RedirectToAction(nameof(Inscripcion));
                }
            }

            // Validar conflictos de horario
            foreach (var nuevoBloque in bloquesSeleccionados)
            {
                bool conflict = inscripcionesActuales.Any(i =>
                    i.IdBloqueHorarioMateriaNavigation.DiaSemana == nuevoBloque.DiaSemana &&
                    i.IdBloqueHorarioMateriaNavigation.HoraInicio < nuevoBloque.HoraFin &&
                    nuevoBloque.HoraInicio < i.IdBloqueHorarioMateriaNavigation.HoraFin
                );

                if (conflict)
                {
                    TempData["Error"] = $"Conflicto de horario detectado con {nuevoBloque.DiaSemana} {nuevoBloque.HoraInicio}-{nuevoBloque.HoraFin}.";
                    return RedirectToAction(nameof(Inscripcion));
                }
            }

            // Guardar inscripciones
            int guardadas = 0;
            foreach (var nuevoBloque in bloquesSeleccionados)
            {
                if (guardadas >= disponibles) break;

                _context.InscripcionAlumnos.Add(new InscripcionAlumno
                {
                    IdAlumno = idAlumno.Value,
                    IdBloqueHorarioMateria = nuevoBloque.Id
                });
                guardadas++;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Materias inscritas correctamente.";
            return RedirectToAction(nameof(Inscripcion));
        }
    }
}
