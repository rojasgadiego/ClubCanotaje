using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Models.Entities.Catalogos;

namespace ClubCanotajeAPI.Repositories.Usuario;

public interface IUsuarioRepository
{
    Task<UsuarioSistema?> GetByUsernameAsync(string username);
    Task RegistrarAccesoAsync(int id);
    Task<bool> ExisteUsernameAsync(string username);
    Task<bool> ExisteRutAsync(string rut);
    Task<bool> ExisteEmailRemadorAsync(string email);
    Task<RolSistema?> GetRolByNombreAsync(string nombre);
    Task<RolSistema?> GetRolByIdAsync(int idRol);
    Task<List<RolSistema>> GetRolesAsync();
    Task<int> GetCategoriaDefaultAsync();
    Task<int> GetEstadoRemadorActivoAsync();
    Task<UsuarioSistema> CrearConRemadorAsync(Remador remador, UsuarioSistema usuario);
    Task<UsuarioSistema> CrearAsync(UsuarioSistema usuario);
    Task<UsuarioSistema?> GetByEmailRemadorAsync(string email);
    Task<Remador?> GetRemadorByEmailAsync(string email);
    Task ActivarUsuarioAsync(UsuarioSistema usuario);
    Task CambiarPasswordAsync(UsuarioSistema usuario, string nuevoPasswordHash);
}