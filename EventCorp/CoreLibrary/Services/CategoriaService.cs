using CoreLibrary.Data;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly EventCorpContext _context;
        private readonly IErrorLogService _errorLogService;

        public CategoriaService(EventCorpContext context, IErrorLogService errorLogService)
        {
            _context = context;
            _errorLogService = errorLogService;
        }

        #region Consultas

        public async Task<CategoriaModel?> ObtenerCategoria(int? id)
        {
            if (id is null)
                return null;

            try
            {
                var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.IdCategoria == id);
                return categoria;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "CategoriaService.ObtenerCategoria", id);
                return null;
            }
        }

        public async Task<IEnumerable<CategoriaModel>> Listado()
        {
            try
            {
                return await _context.Categorias.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "CategoriaService.Listado");
                return Enumerable.Empty<CategoriaModel>();
            }
        }

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.Categorias.AnyAsync(e => e.IdCategoria == id);
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "CategoriaService.Existe", id);
                return false;
            }
        }

        #endregion

        #region CRUD

        public async Task<bool> Guardar(CategoriaModel categoria)
        {
            if (categoria == null)
                return false;

            try
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                await _errorLogService.RegistrarError(dbEx, "CategoriaService.Guardar - Error de base de datos");
                return false;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "CategoriaService.Guardar");
                return false;
            }
        }

        public async Task<bool> Modificar(CategoriaModel categoria)
        {
            if (categoria == null || categoria.IdCategoria == 0)
                return false;

            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException conEx)
            {
                await _errorLogService.RegistrarError(conEx, "CategoriaService.Modificar - Conflicto de concurrencia", categoria.IdCategoria);
                return false;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "CategoriaService.Modificar", categoria.IdCategoria);
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var categoria = await ObtenerCategoria(id);
                if (categoria == null)
                {
                    await _errorLogService.RegistrarError(
                        new Exception($"No se encontró la categoría con ID {id} para eliminar."),
                        "CategoriaService.Eliminar",
                        id
                    );
                    return false;
                }

                _context.Remove(categoria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.RegistrarError(ex, "CategoriaService.Eliminar", id);
                return false;
            }
        }

        #endregion
    }
}
