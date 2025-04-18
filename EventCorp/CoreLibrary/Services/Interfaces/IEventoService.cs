using CoreLibrary.Models;

namespace CoreLibrary.Services.Interfaces
{
    public interface IEventoService
    {
        #region Consultas
        Task<EventoModel?> ObtenerEvento(int? id);
        Task<IEnumerable<EventoModel>> Listado();
        Task<bool> Existe(int id);
        #endregion

        #region CRUD
        Task<bool> Guardar(EventoModel evento);
        Task<bool> Actualizar(EventoModel evento);
        Task<bool> Eliminar(int id);
        #endregion
    }
}
