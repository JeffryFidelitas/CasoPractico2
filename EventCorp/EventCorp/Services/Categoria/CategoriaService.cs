using EventCorp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventCorp.Services.Categoria
{
    public class CategoriaService : ICategoriaService
    {
        private readonly EventCorpContext _context;

        public CategoriaService(EventCorpContext context)
        {
            _context = context;
        }

        public async Task<bool> Eliminar(int id)
        {
            var categoria = await ObtenerCategoria(id);
            if (categoria != null)
            {
                _context.Remove(categoria);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Categorias.AnyAsync(e => e.IdCategoria == id);
        }

        public async Task<bool> Guardar(Models.Categoria categoria)
        {
            try
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Models.Categoria>> Listado()
        {
            var listadoCategorias = await _context.Categorias.ToListAsync();
            return listadoCategorias;
        }

        public async Task<bool> Modificar(Models.Categoria categoria)
        {
            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Models.Categoria> ObtenerCategoria(int? id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.IdCategoria == id);
            return categoria;
        }
    }
}
