using ClubCanotajeAPI.Models.Entities;

namespace ClubCanotajeAPI.Repositories.RemadorRepository;

public interface IRemadorRepository
{
    Task<List<Remador>> GetAllAsync();
    Task<Remador?> GetByIdAsync(int id);
    Task<bool> ExisteRutAsync(string rut, int? excluirId = null);
    Task<bool> ExisteEmailAsync(string email, int? excluirId = null);
    Task<bool> TieneMembresiaVigenteAsync(int idRemador);
    Task AddAsync(Remador r);
    Task UpdateAsync(Remador r);
}