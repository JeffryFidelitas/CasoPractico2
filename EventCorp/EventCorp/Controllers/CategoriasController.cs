using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventCorp.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categorias)
        {
            _categoriaService = categorias;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            try
            {
                var categorias = await _categoriaService.Listado();
                return View(categorias);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al cargar las categorías.");
                return View(new List<CategoriaModel>());
            }
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest("ID no proporcionado.");

            try
            {
                var categoria = await _categoriaService.ObtenerCategoria(id);
                if (categoria == null)
                    return NotFound();

                return View(categoria);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al obtener los detalles de la categoría.");
            }
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoria,Nombre,Descripcion")] CategoriaModel categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            try
            {
                categoria.Estado = true;
                categoria.FechaRegistro = DateTime.UtcNow;

                var resultado = await _categoriaService.Guardar(categoria);
                if (!resultado)
                {
                    ModelState.AddModelError("", "No se pudo guardar la categoría.");
                    return View(categoria);
                }

                TempData["SuccessMessage"] = "Categoría creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error inesperado al guardar la categoría.");
                return View(categoria);
            }
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest("ID no proporcionado.");

            try
            {
                var categoria = await _categoriaService.ObtenerCategoria(id);
                if (categoria == null)
                    return NotFound();

                return View(categoria);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al cargar la categoría para edición.");
            }
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoria,Nombre,Descripcion,Estado,FechaRegistro")] CategoriaModel categoria)
        {
            if (id != categoria.IdCategoria)
                return NotFound("La categoría no coincide con el ID proporcionado.");

            if (!ModelState.IsValid)
                return View(categoria);

            try
            {
                var resultado = await _categoriaService.Modificar(categoria);
                if (!resultado)
                {
                    ModelState.AddModelError("", "No se pudo actualizar la categoría.");
                    return View(categoria);
                }

                TempData["SuccessMessage"] = "Categoría actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoriaExists(categoria.IdCategoria))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error inesperado al actualizar la categoría.");
                return View(categoria);
            }
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest("ID no proporcionado.");

            try
            {
                var categoria = await _categoriaService.ObtenerCategoria(id);
                if (categoria == null)
                    return NotFound();

                return View(categoria);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al cargar la categoría para eliminar.");
            }
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var resultado = await _categoriaService.Eliminar(id);
                if (!resultado)
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar la categoría.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Categoría eliminada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error inesperado al eliminar la categoría.";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<bool> CategoriaExists(int id)
        {
            try
            {
                return await _categoriaService.Existe(id);
            }
            catch
            {
                return false;
            }
        }
    }
}
