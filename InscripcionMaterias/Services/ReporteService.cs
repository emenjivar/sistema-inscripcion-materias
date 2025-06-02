using DinkToPdf;
using DinkToPdf.Contracts;
using InscripcionMaterias.Models;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace InscripcionMaterias.Services
{
    public class ReporteService
    {
        private readonly IConverter _converter;

        public ReporteService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GenerarPdfDesdeHtml(string html)
        {
            // Llama a la versión con opciones usando valores por defecto
            return GenerarPdfDesdeHtml(html, new PdfOptions());
        }

        public byte[] GenerarPdfDesdeHtml(string html, PdfOptions pdfOptions)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = pdfOptions.Orientation,
                PaperSize = pdfOptions.PaperSize,
                Margins = pdfOptions.Margins,
                DocumentTitle = "Reporte de Pensum"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html,
                WebSettings = {
                    DefaultEncoding = "utf-8",
                    LoadImages = true
                },
                HeaderSettings = {
                    FontSize = 9,
                    Right = "Página [page] de [toPage]",
                    Line = true
                },
                FooterSettings = {
                    FontSize = 9,
                    Center = "Generado el: " + DateTime.Now.ToString("dd/MM/yyyy"),
                    Line = true
                }
            };

            var pdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return _converter.Convert(pdfDocument);
        }
    }

}
