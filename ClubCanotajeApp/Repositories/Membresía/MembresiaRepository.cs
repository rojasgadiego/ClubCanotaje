using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClubCanotajeAPI.Repositories.MembresiaRepository
{
    public class MembresiaRepository
    {
        private readonly AppDbContext _db;
        public MembresiaRepository(AppDbContext db) => _db = db;

        public async Task<Membresia?> GetVigenteAsync(int idRemador) =>
            await _db.Membresias
                .Include(m => m.TipoMembresia)
                .Where(m => m.IdRemador == idRemador && m.Activa
                         && DateOnly.FromDateTime(DateTime.Today) >= m.FechaInicio
                         && DateOnly.FromDateTime(DateTime.Today) <= m.FechaFin)
                .FirstOrDefaultAsync();

        public async Task<List<Cuota>> GetCuotasPendientesAsync() =>
            await _db.Cuotas
                .Include(c => c.Estado)
                .Include(c => c.Membresia).ThenInclude(m => m.Remador)
                .Where(c => new[] { "Pendiente", "Vencido" }.Contains(c.Estado.Nombre))
                .OrderBy(c => c.FechaVencimiento)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Cuota?> GetCuotaByIdAsync(int id) =>
            await _db.Cuotas
                .Include(c => c.Estado)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddMembresiaAsync(Membresia m) { await _db.Membresias.AddAsync(m); await _db.SaveChangesAsync(); }
        public async Task AddPagoAsync(Pago p) { await _db.Pagos.AddAsync(p); await _db.SaveChangesAsync(); }
        public async Task UpdateCuotaAsync(Cuota c) { _db.Cuotas.Update(c); await _db.SaveChangesAsync(); }
    }

}