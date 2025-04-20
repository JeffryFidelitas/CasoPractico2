using CoreLibrary.Data;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Services
{
    public class EventoService : IEventoService
    {
        private readonly EventCorpContext _context;
        private readonly IErrorLogService _errorLogService;

        public EventoService(EventCorpContext context, IErrorLogService errorLogService)
        {
            _context = context;
            _errorLogService = errorLogService;
        }

        #region Consultas

        public async Task<EventoModel?> ObtenerEvento(int? id)
        {
            try
            {
                return await _context.Eventos
                    .Include(c => c.Categoria)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "EventoService.ObtenerEvento", id);
                return null;
            }
        }

        public async Task<IEnumerable<EventoModel>> Listado()
        {
            try
            {
                return await _context.Eventos
                    .Include(c => c.Categoria)
                    .Include(e => e.UsuarioRegistro) // Agregar esta línea
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "EventoService.Listado");
                return new List<EventoModel>();
            }
        }

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.Eventos.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "EventoService.Existe", id);
                return false;
            }
        }

        #endregion

        #region CRUD

        public async Task<bool> Guardar(EventoModel evento)
        {
            try
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "EventoService.Guardar");
                return false;
            }
        }

        public async Task<bool> Actualizar(EventoModel evento)
        {
            try
            {
                _context.Update(evento);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "EventoService.Modificar", evento.Id);
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var evento = await ObtenerEvento(id);
                if (evento != null)
                {
                    _context.Remove(evento);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "EventoService.Eliminar", id);
                return false;
            }
        }

        #endregion
    }
}
