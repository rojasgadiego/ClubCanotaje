using ClubCanotajeAPI.Models.Entities;

namespace ClubCanotajeAPI.Repositories.SalidaRepository;

public interface ISalidaRepository
{
    Task<List<Salida>> GetProximasAsync();
    Task<List<Salida>> GetHistorialAsync(DateTime? desde, DateTime? hasta);
    Task<Salida?> GetByIdAsync(int id);
    Task<int> AddAsync(Salida s);
    Task AddParticipanteAsync(SalidaParticipante p);
    Task SaveAsync();
}