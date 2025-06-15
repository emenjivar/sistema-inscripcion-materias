using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using InscripcionMaterias.Models;
using InscripcionMaterias.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InscripcionMaterias.Models.ViewModels;
using DinkToPdf;
using System.Drawing;

public class ReporteController : Controller
{
    private readonly ReporteService _reporteService;
    private readonly GestionDbContext _context;
    private readonly ICompositeViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IServiceProvider _serviceProvider;

    public ReporteController(ReporteService reporteService, GestionDbContext context,
                           ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider,
                           IServiceProvider serviceProvider)
    {
        _reporteService = reporteService;
        _context = context;
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
        _serviceProvider = serviceProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GenerarReportePensum(int id)
    {
        try
        {
            // Obtener las materias del pensum
            var materias = await _context.PensumMaterias
                .Include(pm => pm.IdMateriaNavigation)
                .Include(pm => pm.IdMateriaPrerequisitoNavigation)
                .Where(pm => pm.IdPensum == id)
                .OrderBy(pm => pm.CicloCurricular)
                .ToListAsync();

            // Obtener información del pensum
            var pensum = await _context.Pensums
                .Where(p => p.Id == id)
                .Select(p => new { p.Carrera, p.CantidadCiclos })
                .FirstOrDefaultAsync();

            if (pensum == null)
            {
                return NotFound();
            }

            // Configurar ViewData
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            { "nombreCarrera", pensum.Carrera ?? "Carrera no encontrada" },
            { "cantidadCiclos", pensum.CantidadCiclos },
            { "cantidadMaterias", materias.Count() },

        };

            // Renderizar la vista optimizada para PDF
            var html = await RenderViewToStringAsync("~/Views/Reporte/MateriasAsignadas.cshtml", materias, viewData);

            // Generar el PDF
            var pdfBytes = _reporteService.GenerarPdfDesdeHtml(html, new PdfOptions
            {
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            });

            // Configurar la respuesta para visualización en navegador
            Response.Headers.Append("Content-Disposition", "inline; filename=Pensum.pdf");
            return File(pdfBytes, "application/pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error al generar el reporte");
        }
    }

    private async Task<string> RenderViewToStringAsync(string viewPath, object model, ViewDataDictionary viewData = null)
    {
        var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
        var routeData = new RouteData();
        routeData.Routers.Add(new RouteCollection());

        var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());

        using var sw = new StringWriter();
        var viewResult = _viewEngine.GetView(null, viewPath, false);

        if (!viewResult.Success)
        {
            throw new InvalidOperationException($"No se encontró la vista '{viewPath}'.");
        }

        if (viewData == null)
        {
            viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };
        }
        else
        {
            viewData.Model = model;
        }

        var tempData = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewData,
            tempData,
            sw,
            new HtmlHelperOptions()
        );

        await viewResult.View.RenderAsync(viewContext);
        return sw.ToString();
    }

    [HttpGet]
    public async Task<IActionResult> GenerarReporteInscripcion(int id)
    {
        int? idAlumno = HttpContext.Session.GetInt32("IdAlumno");
        try
        {
            // Obtener las inscripciones del alumno en esta inscripción específica
            var inscripcionesAlumno = await _context.InscripcionAlumnos
                .Include(ia => ia.IdAlumnoNavigation)
                .Include(ia => ia.IdBloqueHorarioMateriaNavigation)
                    .ThenInclude(bhm => bhm.IdMateriaNavigation)
                .Include(ia => ia.IdBloqueHorarioMateriaNavigation)
                    .ThenInclude(bhm => bhm.IdGrupoNavigation)
                .Where(ia => ia.IdBloqueHorarioMateriaNavigation.IdInscripcion == id && ia.IdAlumno == idAlumno)
                .ToListAsync();

            if (!inscripcionesAlumno.Any())
                return NotFound("No hay materias inscritas para esta inscripción.");

            var alumno = inscripcionesAlumno.First().IdAlumnoNavigation;

            var inscripcion = await _context.Inscripcions
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inscripcion == null)
                return NotFound("Inscripción no encontrada.");

            var model = new InscripcionViewModel
            {
                NombreAlumno = alumno.Nombres + " " + alumno.Apellidos,
                Carnet = alumno.Carnet,
                Anio = inscripcion.Anio,
                CicloAcademico = inscripcion.CicloAcademico,
                MateriasSeleccionadas = inscripcionesAlumno.Select(ia => new InscripcionSeleccionadaViewModel
                {
                    MateriaNombre = ia.IdBloqueHorarioMateriaNavigation.IdMateriaNavigation.Nombre,
                    Grupo = ia.IdBloqueHorarioMateriaNavigation.IdGrupoNavigation.Codigo,
                    Horarios = new List<string>
                    {
                        $"{ia.IdBloqueHorarioMateriaNavigation.DiaSemana} {ia.IdBloqueHorarioMateriaNavigation.HoraInicio} - {ia.IdBloqueHorarioMateriaNavigation.HoraFin}"
                    }
                }).ToList()
            };

            var html = await RenderViewToStringAsync("~/Views/Reporte/InscripcionMaterias.cshtml", model);

            var pdfBytes = _reporteService.GenerarPdfDesdeHtml(html, new PdfOptions
            {
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            });

            Response.Headers.Append("Content-Disposition", "inline; filename=InscripcionMaterias.pdf");
            return File(pdfBytes, "application/pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error al generar el reporte");
        }
    }
}
public class PdfOptions
{
    public Orientation Orientation { get; set; } = Orientation.Portrait;
    public PaperKind PaperSize { get; set; } = PaperKind.A4;
    public MarginSettings Margins { get; set; } = new MarginSettings();
}