using System.Web.Mvc;
using CoreLibrary.Models;

namespace CoreLibrary.Services.Interfaces
{
    public interface IUsuarioService
    {
        #region Consultas
        List<SelectListItem> ObtenerRoles();
        Task<List<UsuarioModel>> ObtenerTodos();
        Task<UsuarioModel?> ObtenerPorId(int id);
        Task<UsuarioModel?> ObtenerPorCorreo(string correo);
        Task<UsuarioModel?> ObtenerPorCorreoYContrasena(string correo, string contrasena);
        #endregion

        #region CRUD
        Task<bool> Guardar(UsuarioModel usuario);
        Task<bool> Actualizar(UsuarioModel usuario);
        Task<bool> Eliminar(int id);
        #endregion
    }
}
