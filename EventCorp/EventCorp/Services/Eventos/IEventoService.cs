using EventCorp.Models;

namespace EventCorp.Services.Eventos
{
    public interface IEventoService
    {
        Task<bool> Guardar(Evento evento);
        Task<bool> Modificar(Evento evento);
        Task<bool> Eliminar(int id);
        Task<IEnumerable<Evento>> Listado();
        Task<Evento> ObtenerEvento(int? id);
        Task<bool> Existe(int id);
    }
}
