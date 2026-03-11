using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Dtos.Usuario;

namespace ClubCanotajeAPI.Services.Usuario;

public interface IUsuarioService
{
    Task<ApiResponse<UsuarioInfoDto>> GetInfoCompletaAsync(int id);
}