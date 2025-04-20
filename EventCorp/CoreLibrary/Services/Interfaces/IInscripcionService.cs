using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Services.Interfaces
{
    public interface IInscripcionService
    {
        #region Consultas
        Task<IEnumerable<InscripcionModel>> ObtenerTodas();
        Task<IEnumerable<InscripcionModel>> ObtenerPorEvento(int eventoId);
        Task<IEnumerable<InscripcionModel>> ObtenerPorUsuario(int usuarioId);
        Task<IEnumerable<InscripcionModel>> ObtenerPorOrganizador(int organizadorId);
        Task<InscripcionModel> ObtenerPorId(int id);
        Task<bool> VerificarInscripcionExistente(int usuarioId, int eventoId);
        Task<(bool haySuperposicion, string mensaje)> VerificarSuperposicionEventos(int usuarioId, int eventoId);
        Task<bool> VerificarCupoDisponible(int eventoId);
        #endregion

        #region CRUD
        Task<(bool exito, string mensaje)> Inscribir(int usuarioId, int eventoId);
        Task<bool> MarcarAsistencia(int inscripcionId, bool asistio);
        Task<bool> CancelarInscripcion(int inscripcionId);
        #endregion
    }
}
