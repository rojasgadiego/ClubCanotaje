using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClubCanotajeAPI.Repositories.RemadorRepository
{
    public class RemadorRepository
    {
        private readonly AppDbContext _db;
        public RemadorRepository(AppDbContext db) => _db = db;

        public async Task<List<Remador>> GetAllAsync() =>
            await _db.Remadores
                .Include(r => r.Categoria)
                .Include(r => r.Estado)
                .Include(r => r.Membresias.Where(m => m.Activa))
                    .ThenInclude(m => m.TipoMembresia)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Remador?> GetByIdAsync(int id) =>
            await _db.Remadores
                .Include(r => r.Categoria)
                .Include(r => r.Estado)
                .Include(r => r.Membresias.Where(m => m.Activa))
                    .ThenInclude(m => m.TipoMembresia)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<bool> ExisteRutAsync(string rut, int? excluirId = null) =>
            await _db.Remadores.AnyAsync(r => r.Rut == rut && r.Id != excluirId);

        public async Task<bool> ExisteEmailAsync(string email, int? excluirId = null) =>
            await _db.Remadores.AnyAsync(r => r.Email == email && r.Id != excluirId);

        public async Task<bool> TieneMembresiaVigenteAsync(int idRemador) =>
            await _db.Membresias.AnyAsync(m =>
                m.IdRemador == idRemador && m.Activa &&
                DateOnly.FromDateTime(DateTime.Today) >= m.FechaInicio &&
                DateOnly.FromDateTime(DateTime.Today) <= m.FechaFin);

        public async Task AddAsync(Remador r) { await _db.Remadores.AddAsync(r); await _db.SaveChangesAsync(); }
        public async Task UpdateAsync(Remador r) { _db.Remadores.Update(r); await _db.SaveChangesAsync(); }
    }
}
