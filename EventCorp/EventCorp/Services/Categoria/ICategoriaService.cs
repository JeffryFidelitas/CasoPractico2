using EventCorp.Models;

namespace EventCorp.Services.Categoria
{
    public interface ICategoriaService
    {
        Task<bool> Guardar(EventCorp.Models.Categoria Guardar);
        Task<bool> Modificar(EventCorp.Models.Categoria Cliente);
        Task<bool> Eliminar(int id);
        Task<IEnumerable<EventCorp.Models.Categoria>> Listado();
        Task<EventCorp.Models.Categoria> ObtenerCategoria(int? id);
        Task<bool> Existe(int id);
    }
}
