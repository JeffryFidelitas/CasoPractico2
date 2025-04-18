using CoreLibrary.Models;

namespace CoreLibrary.Services.Interfaces
{
    public interface IErrorLogService
    {
        Task RegistrarError(Exception ex, string origen, int? usuarioId = null, string tipo = "Error");
    }
}
