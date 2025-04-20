using CoreLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventCorp.Controllers
{
    public class InscripcionController : Controller
    {
        private readonly IInscripcionService _inscripcionService;
        private readonly IEventoService _eventoService;
        private readonly IUsuarioService _usuarioService;

        public InscripcionController(
            IInscripcionService inscripcionService,
            IEventoService eventoService,
            IUsuarioService usuarioService)
        {
            _inscripcionService = inscripcionService;
            _eventoService = eventoService;
            _usuarioService = usuarioService;
        }

        // GET: Mostrar eventos disponibles para inscripción
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EventosDisponibles()
        {
            var eventos = await _eventoService.Listado();
            return View(eventos);
        }

        // GET: Mostrar eventos inscritos por el usuario actual
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MisEventos()
        {
            int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var inscripciones = await _inscripcionService.ObtenerPorUsuario(usuarioId);
            return View(inscripciones);
        }

        // POST: Inscribirse a un evento
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscribirse(int eventoId)
        {
            int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var resultado = await _inscripcionService.Inscribir(usuarioId, eventoId);

            if (resultado.exito)
            {
                TempData["SuccessMessage"] = resultado.mensaje;
                return RedirectToAction(nameof(MisEventos));
            }
            else
            {
                TempData["ErrorMessage"] = resultado.mensaje;
                return RedirectToAction(nameof(EventosDisponibles));
            }
        }

        // POST: Cancelar inscripción a un evento
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarInscripcion(int inscripcionId)
        {
            var exito = await _inscripcionService.CancelarInscripcion(inscripcionId);

            if (exito)
            {
                TempData["SuccessMessage"] = "Inscripción cancelada con éxito";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo cancelar la inscripción";
            }

            return RedirectToAction(nameof(MisEventos));
        }

        // GET: Ver asistentes por evento (solo para organizadores y administradores)
        [HttpGet]
        [Authorize(Roles = "Organizador,Administrador")]
        public async Task<IActionResult> AsistentesPorEvento(int eventoId)
        {
            var evento = await _eventoService.ObtenerEvento(eventoId);

            if (evento == null)
            {
                return NotFound();
            }

            // Verificar si el usuario es organizador y es dueño del evento
            string rol = User.FindFirst(ClaimTypes.Role)?.Value;
            int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (rol == "Organizador" && evento.UsuarioRegistroId != usuarioId)
            {
                return Forbid();
            }

            var inscripciones = await _inscripcionService.ObtenerPorEvento(eventoId);
            ViewBag.Evento = evento;

            return View(inscripciones);
        }

        // GET: Ver todos los eventos con sus asistentes (solo para administradores)
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> TodosLosEventos()
        {
            var eventos = await _eventoService.Listado();
            return View(eventos);
        }

        // GET: Ver eventos organizados por el usuario actual (solo para organizadores)
        [HttpGet]
        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> MisEventosOrganizados()
        {
            int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var inscripciones = await _inscripcionService.ObtenerPorOrganizador(usuarioId);
            return View(inscripciones);
        }

        // POST: Marcar asistencia de un usuario a un evento
        [HttpPost]
        [Authorize(Roles = "Organizador,Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarAsistencia(int inscripcionId, bool asistio)
        {
            var exito = await _inscripcionService.MarcarAsistencia(inscripcionId, asistio);

            if (exito)
            {
                TempData["SuccessMessage"] = "Asistencia marcada con éxito";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo marcar la asistencia";
            }

            // Redirigir de vuelta a la página de asistentes
            return RedirectToAction(nameof(AsistentesPorEvento), new { eventoId = Request.Form["eventoId"] });
        }
    }
}