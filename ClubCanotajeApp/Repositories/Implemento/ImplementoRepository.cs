using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace ClubCanotajeAPI.Repositories.ImplementoRepository
{
    public class ImplementoRepository
    {
        private readonly AppDbContext _db;
        public ImplementoRepository(AppDbContext db) => _db = db;

        public async Task<List<Implemento>> GetAllAsync() =>
            await _db.Implementos
                .Include(i => i.Tipo)
                .Include(i => i.Estado)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Implemento>> GetDisponiblesAsync() =>
            await _db.Implementos
                .Include(i => i.Tipo)
                .Include(i => i.Estado)
                .Where(i => i.Estado.Nombre == "Disponible")
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Implemento>> GetDisponiblesPorTipoAsync(int idTipo) =>
            await _db.Implementos
                .Include(i => i.Tipo)
                .Include(i => i.Estado)
                .Where(i => i.Estado.Nombre == "Disponible" && i.IdTipoImplemento == idTipo)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Implemento?> GetByIdAsync(int id) =>
            await _db.Implementos
                .Include(i => i.Tipo)
                .Include(i => i.Estado)
                .FirstOrDefaultAsync(i => i.Id == id);

        public async Task<bool> ExisteCodigoAsync(string codigo, int? excluirId = null) =>
            await _db.Implementos.AnyAsync(i => i.Codigo == codigo && i.Id != excluirId);

        public async Task AddAsync(Implemento i) { await _db.Implementos.AddAsync(i); await _db.SaveChangesAsync(); }
        public async Task UpdateAsync(Implemento i) { _db.Implementos.Update(i); await _db.SaveChangesAsync(); }

        // Asignar implemento a una salida
        public async Task AsignarASalidaAsync(SalidaImplemento si)
        {
            await _db.SalidaImplementos.AddAsync(si);
            await _db.SaveChangesAsync();
        }

        // Devolver implemento de una salida
        public async Task<SalidaImplemento?> GetSalidaImplementoAsync(int idSalida, int idImplemento) =>
            await _db.SalidaImplementos
                .FirstOrDefaultAsync(si => si.IdSalida == idSalida && si.IdImplemento == idImplemento);

        public async Task SaveAsync() => await _db.SaveChangesAsync();
    }
}
