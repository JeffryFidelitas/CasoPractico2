using CoreLibrary.Data;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Services
{
    public class InscripcionService : IInscripcionService
    {
        private readonly EventCorpContext _context;
        private readonly IErrorLogService _errorLogService;

        public InscripcionService(EventCorpContext context, IErrorLogService errorLogService)
        {
            _context = context;
            _errorLogService = errorLogService;
        }

        #region Consultas
        public async Task<IEnumerable<InscripcionModel>> ObtenerTodas()
        {
            try
            {
                return await _context.Inscripciones
                    .Include(i => i.Usuario)
                    .Include(i => i.Evento)
                    .ThenInclude(e => e.Categoria)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "InscripcionService.ObtenerTodas");
                return new List<InscripcionModel>();
            }
        }

        public async Task<IEnumerable<InscripcionModel>> ObtenerPorEvento(int eventoId)
        {
            try
            {
                return await _context.Inscripciones
                    .Where(i => i.EventoId == eventoId)
                    .Include(i => i.Usuario)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "InscripcionService.ObtenerPorEvento", eventoId);
                return new List<InscripcionModel>();
            }
        }

        public async Task<IEnumerable<InscripcionModel>> ObtenerPorUsuario(int usuarioId)
        {
            try
            {
                return await _context.Inscripciones
                    .Where(i => i.UsuarioId == usuarioId)
                    .Include(i => i.Evento)
                    .ThenInclude(e => e.Categoria)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "InscripcionService.ObtenerPorUsuario", usuarioId);
                return new List<InscripcionModel>();
            }
        }

        public async Task<IEnumerable<InscripcionModel>> ObtenerPorOrganizador(int organizadorId)
        {
            try
            {
                var eventosDelOrganizador = await _context.Eventos
                    .Where(e => e.UsuarioRegistroId == organizadorId)
                    .Select(e => e.Id)
                    .ToListAsync();

                return await _context.Inscripciones
                    .Where(i => eventosDelOrganizador.Contains(i.EventoId))
                    .Include(i => i.Usuario)
                    .Include(i => i.Evento)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "InscripcionService.ObtenerPorOrganizador", organizadorId);
                return new List<InscripcionModel>();
            }
        }

        public async Task<InscripcionModel> ObtenerPorId(int id)
        {
            try
            {
                return await _context.Inscripciones
                    .Include(i => i.Usuario)
                    .Include(i => i.Evento)
                    .FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "InscripcionService.ObtenerPorId", id);
                return null;
            }
        }

        public async Task<bool> VerificarInscripcionExistente(int usuarioId, int eventoId)
        {
            try
            {
                return await _context.Inscripciones
                    .AnyAsync(i => i.UsuarioId == usuarioId && i.EventoId == eventoId);
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "InscripcionService.VerificarInscripcionExistente");
                return false;
            }
        }

        public async Task<(bool haySuperposicion, string mensaje)> VerificarSuperposicionEventos(int usuarioId, int eventoId)
        {
            try
            {
                // Obtener el evento al que desea inscribirse
                var nuevoEvento = await _context.Eventos.FindAsync(eventoId);
                if (nuevoEvento == null)
                    return (false, "Evento no encontrado");

                // Calcular inicio y fin del nuevo evento
                var nuevoInicio = nuevoEvento.Fecha.Add(nuevoEvento.Hora);
                var nuevoFin = nuevoInicio.AddMinutes(nuevoEvento.Duracion);

                // Registrar para diagnóstico
                await _errorLogService.RegistrarError(
                    new Exception("Log informativo"),
                    $"Verificando superposición para evento {eventoId}, inicio: {nuevoInicio}, fin: {nuevoFin}",
                    usuarioId);

                // Obtener todas las inscripciones del usuario
                var inscripciones = await _context.Inscripciones
                    .Where(i => i.UsuarioId == usuarioId)
                    .Include(i => i.Evento)
                    .ToListAsync();

                // Registrar el número de inscripciones encontradas
                await _errorLogService.RegistrarError(
                    new Exception("Log informativo"),
                    $"Usuario {usuarioId} tiene {inscripciones.Count} inscripciones",
                    null);

                // Si no hay inscripciones, no puede haber superposición
                if (inscripciones.Count == 0)
                    return (false, "No tiene inscripciones previas");

                // Verificar si hay superposición con algún evento existente
                foreach (var inscripcion in inscripciones)
                {
                    var eventoExistente = inscripcion.Evento;

                    // Si el evento es null, continuar con el siguiente
                    if (eventoExistente == null)
                        continue;

                    var existenteInicio = eventoExistente.Fecha.Add(eventoExistente.Hora);
                    var existenteFin = existenteInicio.AddMinutes(eventoExistente.Duracion);

                    // Registrar detalles del evento a comparar
                    await _errorLogService.RegistrarError(
                        new Exception("Log informativo"),
                        $"Comparando con evento {eventoExistente.Id}, inicio: {existenteInicio}, fin: {existenteFin}",
                        null);

                    // Verificar superposición con lógica más clara
                    bool hayConflicto = (nuevoInicio < existenteFin) && (nuevoFin > existenteInicio);

                    if (hayConflicto)
                    {
                        return (true, $"Conflicto con evento: {eventoExistente.Titulo}");
                    }
                }

                return (false, "No hay superposición");
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(
                    ex,
                    $"InscripcionService.Inscribir - UsuarioId: {usuarioId}, EventoId: {eventoId}",
                    usuarioId); // Pasamos solo el usuarioId como tercer parámetro

                return (false, $"Ocurrió un error al procesar la inscripción: {ex.Message}");
            }
        }

        public async Task<bool> VerificarCupoDisponible(int eventoId)
        {
            try
            {
                var evento = await _context.Eventos.FindAsync(eventoId);
                if (evento == null) return false;

                var inscritosCount = await _context.Inscripciones
                    .Where(i => i.EventoId == eventoId)
                    .CountAsync();

                return inscritosCount < evento.CupoMaximo;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "InscripcionService.VerificarCupoDisponible", eventoId);
                return false; // Devolver false para prevenir inscripción en caso de error
            }
        }
        #endregion

        #region CRUD
        public async Task<(bool exito, string mensaje)> Inscribir(int usuarioId, int eventoId)
        {
            try
            {
                // Verificar si el usuario existe
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                    return (false, "Usuario no encontrado");

                // Verificar si el evento existe
                var evento = await _context.Eventos.FindAsync(eventoId);
                if (evento == null)
                    return (false, "Evento no encontrado");

                // Verificar si ya existe la inscripción
                var inscripcionExistente = await VerificarInscripcionExistente(usuarioId, eventoId);
                if (inscripcionExistente)
                    return (false, "Ya estás inscrito en este evento");

                // Verificar superposición con otros eventos
                var (haySuperposicion, mensajeSuperposicion) = await VerificarSuperposicionEventos(usuarioId, eventoId);
                if (haySuperposicion)
                    return (false, $"El evento se superpone con otro evento al que ya estás inscrito. {mensajeSuperposicion}");

                // Verificar disponibilidad de cupo
                var hayCupo = await VerificarCupoDisponible(eventoId);
                if (!hayCupo)
                    return (false, "El evento ha alcanzado su cupo máximo");

                // Crear inscripción
                var nuevaInscripcion = new InscripcionModel
                {
                    UsuarioId = usuarioId,
                    EventoId = eventoId,
                    FechaInscripcion = DateTime.Now,
                    Asistio = false
                };

                _context.Inscripciones.Add(nuevaInscripcion);
                await _context.SaveChangesAsync();

                return (true, "Inscripción realizada con éxito");
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(
                    ex,
                    $"InscripcionService.VerificarSuperposicionEventos - Error: UsuarioId={usuarioId}, EventoId={eventoId}",
                    null); // Pasamos null como tercer parámetro

                return (true, $"Error al verificar superposición: {ex.Message}");
            }
        }

        public async Task<bool> MarcarAsistencia(int inscripcionId, bool asistio)
        {
            try
            {
                var inscripcion = await _context.Inscripciones.FindAsync(inscripcionId);
                if (inscripcion == null)
                    return false;

                inscripcion.Asistio = asistio;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "InscripcionService.MarcarAsistencia", inscripcionId);
                return false;
            }
        }

        public async Task<bool> CancelarInscripcion(int inscripcionId)
        {
            try
            {
                var inscripcion = await _context.Inscripciones.FindAsync(inscripcionId);
                if (inscripcion == null)
                    return false;

                _context.Inscripciones.Remove(inscripcion);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "InscripcionService.CancelarInscripcion", inscripcionId);
                return false;
            }
        }
        #endregion
    }
}