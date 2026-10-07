using Biblioteca.We.Models;
using Biblioteca.We.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.We.Controllers
{
    public class LibrosController : Controller
    {
        private readonly LibroRepositorio _libroRepositorio;

        public LibrosController(LibroRepositorio libroRepositorio)
        {
            _libroRepositorio = libroRepositorio;
        }

        // GET: /Libros  o  /Libros?titulo=amor
        public async Task<IActionResult> Index(string? titulo)
        {
            IEnumerable<Libro> libros = string.IsNullOrWhiteSpace(titulo)
                ? await _libroRepositorio.ListarAsync()
                : await _libroRepositorio.BuscarPorTituloAsync(titulo.Trim());

            ViewData["Busqueda"] = titulo?.Trim();
            return View(libros);
        }

        // GET: /Libros/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();

            return View(libro);
        }

        // GET: /Libros/Create
        public async Task<IActionResult> Create()
        {
            await CargarAutoresAsync();
            return View(new Libro { Ejemplares = 1 });
        }

        // POST: /Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                await CargarAutoresAsync(libro.AutorId);
                return View(libro);
            }

            await _libroRepositorio.InsertarAsync(libro);
            TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" se registró correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Libros/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();

            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        // POST: /Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro)
        {
            if (id != libro.LibroId) return BadRequest();

            if (!ModelState.IsValid)
            {
                await CargarAutoresAsync(libro.AutorId);
                return View(libro);
            }

            var existe = await _libroRepositorio.ObtenerPorIdAsync(id);
            if (existe == null) return NotFound();

            await _libroRepositorio.ActualizarAsync(libro);
            TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Libros/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();

            return View(libro);
        }

        // POST: /Libros/Delete/5  (eliminación lógica)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();

            await _libroRepositorio.EliminarAsync(id);
            TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // Llena la lista desplegable de autores
        private async Task CargarAutoresAsync(int? autorSeleccionado = null)
        {
            var autores = await _libroRepositorio.ListarAutoresAsync();
            ViewData["Autores"] = new SelectList(autores, "AutorId", "Nombre", autorSeleccionado);
        }
    }
}