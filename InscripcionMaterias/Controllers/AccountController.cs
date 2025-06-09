using InscripcionMaterias.CustomModels;
using InscripcionMaterias.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InscripcionMaterias.Controllers
{
    public class AccountController : Controller
    {
        private readonly GestionDbContext _context;

        public AccountController(GestionDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Opcional: Si el usuario ya está en sesión, redirigirlo
            if (HttpContext.Session.GetString("Username") != null)
            {
                // Podrías redirigir según el rol guardado en sesión
                if (HttpContext.Session.GetString("Rol") == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (HttpContext.Session.GetString("Rol") == "Alumno")
                {
                    return RedirectToAction("Index", "Alumno");
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // O a una página por defecto
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (usuario != null)
                {
                    if (usuario.Password != model.Password)
                    {
                        ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                        return View(model);
                    }

                    // Guardar datos comunes
                    HttpContext.Session.SetString("Username", usuario.Username);
                    HttpContext.Session.SetString("Rol", usuario.Rol);
                    HttpContext.Session.SetInt32("UserId", usuario.Id);

                    // Si es alumno, buscar su ID de alumno y guardarlo en sesión
                    if (usuario.Rol.ToLower() == "alumno")
                    {
                        var alumno = await _context.Alumnos
                            .FirstOrDefaultAsync(a => a.IdUsuario == usuario.Id);

                        if (alumno != null)
                        {
                            HttpContext.Session.SetInt32("IdAlumno", alumno.Id);
                        }
                        else
                        {
                            // Si no existe el alumno, puedes redirigir con error o manejarlo como creas conveniente
                            ModelState.AddModelError("", "No se encontró el registro de alumno asociado a este usuario.");
                            return View(model);
                        }

                        return RedirectToAction("Index", "Home");
                    }

                    // Si es admin, redirigir a panel de administración
                    if (usuario.Rol.ToLower() == "admin")
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    // Si el rol es desconocido, redirigir a home
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                }
            }

            return View(model);
        }


        // GET: /Account/Logout (Para cerrar sesión)
        [HttpGet]
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Elimina todos los datos de la sesión
            return RedirectToAction("Login", "Account");
        }
    }
}
