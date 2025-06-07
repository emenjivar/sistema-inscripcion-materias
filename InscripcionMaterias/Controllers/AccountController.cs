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

                    HttpContext.Session.SetString("Username", usuario.Username);
                    HttpContext.Session.SetString("Rol", usuario.Rol);
                    HttpContext.Session.SetString("UserId", usuario.Id.ToString());

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
