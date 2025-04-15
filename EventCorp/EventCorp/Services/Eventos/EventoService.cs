using EventCorp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventCorp.Services.Eventos
{
    public class EventoService : IEventoService
    {
        private readonly EventCorpContext _context;

        public EventoService(EventCorpContext context)
        {
            _context = context;
        }
        public async Task<bool> Eliminar(int id)
        {
            var evento = await ObtenerEvento(id);
            if (evento != null)
            {
                _context.Remove(evento);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Eventos.AnyAsync(e => e.IdEvento == id);
        }

        public async Task<bool> Guardar(Evento evento)
        {
            try
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Evento>> Listado()
        {
            var listadoEventos = await _context.Eventos
                .Include(c => c.Categoria)
                .ToListAsync();
            return listadoEventos;
        }

        public async Task<bool> Modificar(Evento evento)
        {
            try
            {
                _context.Update(evento);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Evento> ObtenerEvento(int? id)
        {
            var evento = await _context.Eventos
                .Include(c => c.Categoria)
                .FirstOrDefaultAsync(c => c.IdEvento == id);
            return evento;
        }
    }
}
