using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Dtos.Salida;

namespace ClubCanotajeAPI.Services;

public interface ISalidaService
{
    Task<ApiResponse<List<SalidaListDto>>> GetProximasAsync();
    Task<ApiResponse<List<SalidaListDto>>> GetHistorialAsync(DateTime? desde, DateTime? hasta);
    Task<ApiResponse<SalidaDetalleDto>> GetByIdAsync(int id);
    Task<ApiResponse<SalidaDetalleDto>> CrearReservaAsync(CrearReservaDto dto, int idResponsable);
    Task<ApiResponse> AgregarParticipanteAsync(int idSalida, AgregarParticipanteDto dto);
    Task<ApiResponse> IniciarAsync(int idSalida, IniciarSalidaDto dto);
    Task<ApiResponse> FinalizarAsync(int idSalida, FinalizarSalidaDto dto);
    Task<ApiResponse> CancelarAsync(int idSalida, CancelarReservaDto dto, int canceladoPor);
}