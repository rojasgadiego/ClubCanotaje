using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Dtos.Usuario;
using ClubCanotajeAPI.Repositories.Usuario;

namespace ClubCanotajeAPI.Services.Usuario;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;

    public UsuarioService(IUsuarioRepository repo) => _repo = repo;

    public async Task<ApiResponse<UsuarioInfoDto>> GetInfoCompletaAsync(int id)
    {
        var dto = await _repo.GetInfoCompletaAsync(id);

        if (dto is null)
            return ApiResponse<UsuarioInfoDto>.Fail($"Usuario con ID {id} no encontrado.");

        return ApiResponse<UsuarioInfoDto>.Ok(dto);
    }
}