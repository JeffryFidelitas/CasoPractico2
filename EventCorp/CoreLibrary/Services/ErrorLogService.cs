using CoreLibrary.Data;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;

namespace CoreLibrary.Services
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly EventCorpContext _context;

        public ErrorLogService(EventCorpContext context)
        {
            _context = context;
        }

        public async Task RegistrarError(Exception ex, string origen, int? usuarioId = null, string tipo = "Error")
        {
            var error = new ErrorLog
            {
                UsuarioId = usuarioId,
                // Se concatena el mensaje de error con el mensaje de la excepción interna, si existe
                Mensaje = $"{ex.Message}" + (ex.InnerException != null ? $" | Inner: {ex.InnerException.Message}" : ""),
                // Se concatena el stack trace con el stack trace de la excepción interna, si existe
                StackTrace = $"{ex.StackTrace}" + (ex.InnerException != null ? $"\n\nInner StackTrace:\n{ex.InnerException.StackTrace}" : ""),
                Origen = origen,
                Tipo = tipo,
                Fecha = DateTime.UtcNow
            };

            await Registrar(error);
        }

        private async Task Registrar(ErrorLog log)
        {
            try
            {
                _context.Errores.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar el log: {ex.Message}");
            }
        }
    }
}
