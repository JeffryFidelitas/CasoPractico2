using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLibrary.Services.Interfaces;
using CoreLibrary.Models;
using CoreLibrary.Models.ViewModels;

namespace EventCorp.Controllers
{
    public class EventosController : Controller
    {
        private readonly IEventoService _eventoService;
        private readonly ICategoriaService _categoriaService;

        public EventosController(IEventoService eventos, ICategoriaService categorias)
        {
            _eventoService = eventos;
            _categoriaService = categorias;
        }

        // GET: Eventos
        public async Task<IActionResult> Index()
        {
            return View(await _eventoService.Listado());
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _eventoService.ObtenerEvento(id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // GET: Eventos/Create
        public async Task<IActionResult> CreateAsync()
        {
            var categorias = await _categoriaService.Listado();
            ViewBag.Categorias = new SelectList(categorias, "IdCategoria", "Nombre");
            return View();
        }

        // POST: Eventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Convertir Hora y Duración desde string a TimeSpan
            if (!TimeSpan.TryParseExact(model.Hora, "hh\\:mm", null, out var hora))
            {
                ModelState.AddModelError("Hora", "Formato de hora inválido.");
                return View(model);
            }

            // Mapear al modelo real
            var evento = new EventoModel
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                CategoriaId = model.CategoriaId,
                Fecha = model.Fecha,
                Hora = hora,
                Duracion = model.Duracion,
                Ubicacion = model.Ubicacion,
                CupoMaximo = model.MaxAsistentes,
                FechaRegistro = DateTime.Now
            };

            var result = await _eventoService.Guardar(evento);
            if (!result)
            {
                ModelState.AddModelError("", "No se pudo guardar el evento.");
                return View(model);
            }
            var categorias = await _categoriaService.Listado();
            ViewBag.Categorias = new SelectList(categorias, "IdCategoria", "Nombre");
            return RedirectToAction("Index");
        }

        // GET: Eventos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _eventoService.ObtenerEvento(id);
            if (evento == null)
            {
                return NotFound();
            }
            var categorias = await _categoriaService.Listado();
            ViewBag.Categorias = new SelectList(categorias, "IdCategoria", "Nombre");
            return View(evento);
        }

        // POST: Eventos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEvento,Titulo,Descripcion,CategoriaId,Fecha,Hora,Duracion,Ubicacion,MaxAsistentes,FechaRegistro")] EventoModel evento)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _eventoService.Actualizar(evento);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await EventoExists(evento.Id))
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
            return View(evento);
        }

        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _eventoService.ObtenerEvento(id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _eventoService.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EventoExists(int id)
        {
            return await _eventoService.Existe(id);
        }
    }
}
