using InscripcionMaterias.Models;
using Microsoft.AspNetCore.Mvc;

namespace InscripcionMaterias.Controllers
{
    public class BloqueHorarioMaterialController : Controller
    {
        private readonly GestionDbContext _context;

        public BloqueHorarioMaterialController(GestionDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Materias = _context.Materia.ToList();
            ViewBag.Grupos = _context.GrupoClases.ToList();
            ViewBag.Pensums= _context.Pensums.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AgregarBloque(BloqueHorarioMaterial model)
        {
            if (ModelState.IsValid)
            {
                _context.BloqueHorarioMaterials.Add(model);
                _context.SaveChanges();
                return PartialView("_ListaBloques", _context.BloqueHorarioMaterials.ToList());
            }
            return BadRequest();
        }
    }
}
