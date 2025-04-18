using CoreLibrary.Models;

namespace CoreLibrary.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<bool> Guardar(CategoriaModel Guardar);
        Task<bool> Modificar(CategoriaModel Cliente);
        Task<bool> Eliminar(int id);
        Task<IEnumerable<CategoriaModel>> Listado();
        Task<CategoriaModel> ObtenerCategoria(int? id);
        Task<bool> Existe(int id);
    }
}
