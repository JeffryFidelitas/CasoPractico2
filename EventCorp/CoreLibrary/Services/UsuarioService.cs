using CoreLibrary.Data;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly EventCorpContext _context;
        private readonly IErrorLogService _errorLogService;

        public UsuarioService(EventCorpContext context, IErrorLogService errorLogService)
        {
            _context = context;
            _errorLogService = errorLogService;
        }

        #region Consultas

        public async Task<List<UsuarioModel>> ObtenerTodos()
        {
            try
            {
                return await _context.Usuarios.ToListAsync();
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "UsuarioService.ObtenerTodos", null);
                return new List<UsuarioModel>();
            }
        }

        public async Task<UsuarioModel?> ObtenerPorId(int id)
        {
            try
            {
                return await _context.Usuarios.FindAsync(id);
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "UsuarioService.ObtenerPorId", id);
                return null;
            }
        }

        public async Task<UsuarioModel?> ObtenerPorCorreo(string correo)
        {
            try
            {
                return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "UsuarioService.ObtenerPorCorreo", null);
                return null;
            }
        }

        public async Task<UsuarioModel?> ObtenerPorCorreoYContrasena(string correo, string contrasena)
        {
            try
            {
                return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo && u.Contrasena == contrasena);
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "UsuarioService.ObtenerPorCorreoYContrasena", null);
                return null;
            }
        }

        #endregion

        #region CRUD

        public async Task<bool> Guardar(UsuarioModel usuario)
        {
            try
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "UsuarioService.Guardar", usuario.Id);
                return false;
            }
        }

        public async Task<bool> Actualizar(UsuarioModel usuario)
        {
            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "UsuarioService.Actualizar", usuario.Id);
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var usuario = await ObtenerPorId(id);
                if (usuario != null)
                {
                    _context.Remove(usuario);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "UsuarioService.Eliminar", id);
                return false;
            }
        }

        #endregion
    }
}
