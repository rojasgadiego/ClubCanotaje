using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClubCanotajeAPI.Repositories.SalidaRepository
{
    public class SalidaRepository
    {
        private readonly AppDbContext _db;
        public SalidaRepository(AppDbContext db) => _db = db;

        public async Task<List<Salida>> GetProximasAsync() =>
            await _db.Salidas
                .Include(s => s.Canoa).ThenInclude(c => c.TipoCanoa)
                .Include(s => s.Estado)
                .Include(s => s.Responsable)
                .Include(s => s.Participantes)
                .Where(s => new[] { "Reservada", "Confirmada" }.Contains(s.Estado.Nombre)
                         && s.FechaHoraProgramada >= DateTime.Now)
                .OrderBy(s => s.FechaHoraProgramada)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Salida>> GetHistorialAsync(DateTime? desde, DateTime? hasta) =>
            await _db.Salidas
                .Include(s => s.Canoa).ThenInclude(c => c.TipoCanoa)
                .Include(s => s.Estado)
                .Include(s => s.Responsable)
                .Include(s => s.Participantes)
                .Where(s => s.Estado.Nombre == "Finalizada"
                         && (desde == null || s.FechaHoraProgramada >= desde)
                         && (hasta == null || s.FechaHoraProgramada <= hasta))
                .OrderByDescending(s => s.FechaHoraProgramada)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Salida?> GetByIdAsync(int id) =>
            await _db.Salidas
                .Include(s => s.Canoa).ThenInclude(c => c.TipoCanoa)
                .Include(s => s.Estado)
                .Include(s => s.Responsable)
                .Include(s => s.Participantes).ThenInclude(p => p.Remador)
                .Include(s => s.Participantes).ThenInclude(p => p.Rol)
                .Include(s => s.Implementos)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<int> AddAsync(Salida s)
        {
            await _db.Salidas.AddAsync(s);
            await _db.SaveChangesAsync();
            return s.Id;
        }

        public async Task AddParticipanteAsync(SalidaParticipante p)
        {
            await _db.SalidaParticipantes.AddAsync(p);
            await _db.SaveChangesAsync();
        }

        public async Task SaveAsync() => await _db.SaveChangesAsync();
    }
}
