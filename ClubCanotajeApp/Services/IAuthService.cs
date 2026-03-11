using ClubCanotajeAPI.Models.Dtos.Auth;

namespace ClubCanotajeAPI.Services;

public interface IAuthService
{
    Task<(LoginResponse response, string token)> LoginAsync(LoginRequest dto);
    Task<RegistrarUsuarioResponse> RegistroPublicoAsync(RegistroPublicoRequest dto);
    Task<RegistrarUsuarioResponse> RegistroAdminAsync(RegistroAdminRequest dto);
    Task VerificarEmailAsync(string email, string codigo);
    Task ReenviarCodigoAsync(string email);
    Task SolicitarResetPasswordAsync(string email);
    Task ResetPasswordAsync(string email, string codigo, string nuevaPassword);
    Task<List<RolDto>> GetRolesAsync();
}