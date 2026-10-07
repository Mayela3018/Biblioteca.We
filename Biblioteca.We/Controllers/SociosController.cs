using Biblioteca.We.Models;
using Biblioteca.We.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.We.Controllers
{
    public class SociosController : Controller
    {
        private readonly SocioRepositorio _socioRepositorio;

        public SociosController(SocioRepositorio socioRepositorio)
        {
            _socioRepositorio = socioRepositorio;
        }

        // GET: /Socios
        public async Task<IActionResult> Index()
        {
            var socios = await _socioRepositorio.ListarAsync();
            return View(socios);
        }

        // GET: /Socios/Create
        public IActionResult Create()
        {
            return View(new Socio());
        }

        // POST: /Socios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Socio socio)
        {
            if (!ModelState.IsValid)
            {
                return View(socio);
            }

            var insertado = await _socioRepositorio.InsertarAsync(socio);
            if (!insertado)
            {
                ModelState.AddModelError(nameof(Socio.DNI), "Ya existe un socio registrado con ese DNI.");
                return View(socio);
            }

            TempData["Mensaje"] = $"El socio \"{socio.Nombre}\" se registró correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}