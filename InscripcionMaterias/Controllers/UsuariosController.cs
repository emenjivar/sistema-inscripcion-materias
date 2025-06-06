using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InscripcionMaterias.Models;
using InscripcionMaterias.CustomModels;

namespace InscripcionMaterias.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly GestionDbContext _context;

        public UsuariosController(GestionDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuariosDB = await _context.Usuarios.ToListAsync();
            return View(usuariosDB);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Email,Nombres,Apellidos,Password,Rol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var usuarios = await _context.Usuarios.ToListAsync();
            return View("Index", usuarios);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Por seguridad, no se mapea aca el password.
            var usuarioEditModel = dbEntityToModel(usuario);
            return View(usuarioEditModel);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,Nombres,Apellidos,NewPassword,Rol")] UsuarioEditModel usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioToUpdate = await _context.Usuarios.FindAsync(usuario.Id);

                    if (usuarioToUpdate == null)
                    {
                        return NotFound();
                    }

                    usuarioToUpdate.Username = usuario.Username;
                    usuarioToUpdate.Email = usuario.Email;
                    usuarioToUpdate.Nombres = usuario.Nombres;
                    usuarioToUpdate.Apellidos = usuario.Apellidos;
                    usuarioToUpdate.Rol = usuario.Rol;

                    // Actualizar password solamente si el valor se envia desde el formulario
                    if (!string.IsNullOrEmpty(usuario.NewPassword))
                    {
                        usuarioToUpdate.Password = usuario.NewPassword;
                    }
                    _context.Update(usuarioToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        private UsuarioEditModel dbEntityToModel(Usuario usuarioDB)
        {
            return new UsuarioEditModel
            {
                Username = usuarioDB.Username,
                Email = usuarioDB.Email,
                Nombres = usuarioDB.Nombres,
                Apellidos = usuarioDB.Apellidos,
                Rol = usuarioDB.Rol
            };
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        public async Task<IActionResult> UsuariosTabla()
        {
            var usuariosDB = await _context.Usuarios.ToListAsync();
            return PartialView("_UsuariosTabla", usuariosDB);
        }
    }
}
