using Biblioteca.We.Models;
using Biblioteca.We.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.We.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly PrestamoRepositorio _prestamoRepositorio;

        public PrestamosController(PrestamoRepositorio prestamoRepositorio)
        {
            _prestamoRepositorio = prestamoRepositorio;
        }

        // GET: /Prestamos/Reporte?desde=2026-01-01&hasta=2026-12-31
        public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
        {
            var fechaDesde = desde ?? DateTime.Today.AddYears(-1);
            var fechaHasta = hasta ?? DateTime.Today;

            // Conservar el rango elegido para mostrarlo en el formulario
            ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
            ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");

            if (fechaDesde > fechaHasta)
            {
                ViewData["Error"] = "La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'.";
                return View(Enumerable.Empty<PrestamoReporte>());
            }

            var prestamos = await _prestamoRepositorio.ReportePorFechasAsync(fechaDesde, fechaHasta);
            return View(prestamos);
        }
    }
}