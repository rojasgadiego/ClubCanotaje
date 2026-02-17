using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClubCanotajeAPI.Repositories.CanoaRepository
{
    public class CanoaRepository
    {
        private readonly AppDbContext _db;
        public CanoaRepository(AppDbContext db) => _db = db;

        public async Task<List<Canoa>> GetAllAsync() =>
            await _db.Canoas
                .Include(c => c.TipoCanoa)
                .Include(c => c.Estado)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Canoa>> GetDisponiblesAsync() =>
            await _db.Canoas
                .Include(c => c.TipoCanoa)
                .Include(c => c.Estado)
                .Where(c => c.Estado.Nombre == "Disponible")
                .AsNoTracking()
                .ToListAsync();

        public async Task<Canoa?> GetByIdAsync(int id) =>
            await _db.Canoas
                .Include(c => c.TipoCanoa)
                .Include(c => c.Estado)
                .FirstOrDefaultAsync(c => c.Id == id);

        /// <summary>
        /// Retorna canoas disponibles en un horario específico.
        /// Filtra por:
        /// 1. Estado de canoa = Disponible
        /// 2. No tiene reservas activas que solapen con el horario solicitado
        /// </summary>
        public async Task<List<Canoa>> GetDisponiblesPorHorarioAsync(DateTime fechaHoraInicio,int duracionMinutos = 120)
        {
            var fechaHoraFin = fechaHoraInicio.AddMinutes(duracionMinutos);

            // 1. Estados de salida que bloquean la canoa
            var estadosBloqueantes = await _db.EstadosSalida
                .Where(e => new[] { "Reservada", "Confirmada", "En curso" }.Contains(e.Nombre))
                .Select(e => e.Id)
                .ToListAsync();

            // 2. IDs de canoas que tienen conflicto en ese horario
            var canoasOcupadas = await _db.Salidas
                .Where(s =>
                    estadosBloqueantes.Contains(s.IdEstado) &&
                    fechaHoraInicio < s.FechaHoraProgramada.AddMinutes(s.DuracionEstimadaMin ?? 120) &&
                    fechaHoraFin > s.FechaHoraProgramada)
                .Select(s => s.IdCanoa)
                .Distinct()
                .ToListAsync();

            // 3. Canoas disponibles = estado Disponible + no están ocupadas
            return await _db.Canoas
                .Include(c => c.TipoCanoa)
                .Include(c => c.Estado)
                .Where(c =>
                    c.Estado.Nombre == "Disponible" &&
                    !canoasOcupadas.Contains(c.Id))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
