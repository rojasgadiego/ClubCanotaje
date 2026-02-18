using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Models.Entities.Catalogos;
using Microsoft.EntityFrameworkCore;


namespace ClubCanotajeAPI.Repositories.EventoRepository
{
    public class EventoRepository
    {
        private readonly AppDbContext _db;
        public EventoRepository(AppDbContext db) => _db = db;

        // ── Eventos ───────────────────────────────────────────────

        public async Task<List<Evento>> GetAllAsync() =>
            await _db.Eventos
                .Include(e => e.Tipo)
                .Include(e => e.Estado)
                .Include(e => e.Inscripciones)
                .AsNoTracking()
                .OrderByDescending(e => e.FechaInicio)
                .ToListAsync();

        public async Task<List<Evento>> GetProximosAsync() =>
            await _db.Eventos
                .Include(e => e.Tipo)
                .Include(e => e.Estado)
                .Include(e => e.Inscripciones)
                .Where(e => e.Estado.Nombre != "Finalizado" &&
                            e.Estado.Nombre != "Cancelado" &&
                            e.FechaInicio >= DateOnly.FromDateTime(DateTime.Today))
                .AsNoTracking()
                .OrderBy(e => e.FechaInicio)
                .ToListAsync();

        public async Task<Evento?> GetByIdAsync(int id) =>
            await _db.Eventos
                .Include(e => e.Tipo)
                .Include(e => e.Estado)
                .Include(e => e.Inscripciones).ThenInclude(i => i.Equipo)
                .Include(e => e.Inscripciones).ThenInclude(i => i.Remador)
                .Include(e => e.Inscripciones).ThenInclude(i => i.Canoa)
                .Include(e => e.Inscripciones).ThenInclude(i => i.Resultado)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<int> AddAsync(Evento evento)
        {
            await _db.Eventos.AddAsync(evento);
            await _db.SaveChangesAsync();
            return evento.Id;
        }

        public async Task UpdateAsync(Evento evento)
        {
            _db.Eventos.Update(evento);
            await _db.SaveChangesAsync();
        }

        // ── Inscripciones ─────────────────────────────────────────

        public async Task<EventoInscripcion?> GetInscripcionByIdAsync(int id) =>
            await _db.EventoInscripciones
                .Include(i => i.Evento)
                .Include(i => i.Equipo)
                .Include(i => i.Remador)
                .Include(i => i.Canoa)
                .Include(i => i.Resultado)
                .FirstOrDefaultAsync(i => i.Id == id);

        public async Task<bool> YaInscritoEquipoAsync(int idEvento, int idEquipo) =>
            await _db.EventoInscripciones
                .AnyAsync(i => i.IdEvento == idEvento && i.IdEquipo == idEquipo);

        public async Task<bool> YaInscritoRemadorAsync(int idEvento, int idRemador) =>
            await _db.EventoInscripciones
                .AnyAsync(i => i.IdEvento == idEvento && i.IdRemador == idRemador);

        public async Task<int> AddInscripcionAsync(EventoInscripcion inscripcion)
        {
            await _db.EventoInscripciones.AddAsync(inscripcion);
            await _db.SaveChangesAsync();
            return inscripcion.Id;
        }

        public async Task UpdateInscripcionAsync(EventoInscripcion inscripcion)
        {
            _db.EventoInscripciones.Update(inscripcion);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExisteNumeroLargadaAsync(int idEvento, int numero, int? excluirId = null) =>
            await _db.EventoInscripciones
                .AnyAsync(i => i.IdEvento == idEvento &&
                              i.NumeroLargada == numero &&
                              i.Id != excluirId);

        // ── Resultados ────────────────────────────────────────────

        public async Task<EventoResultado?> GetResultadoByInscripcionAsync(int idInscripcion) =>
            await _db.EventoResultados
                .FirstOrDefaultAsync(r => r.IdInscripcion == idInscripcion);

        public async Task AddResultadoAsync(EventoResultado resultado)
        {
            await _db.EventoResultados.AddAsync(resultado);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateResultadoAsync(EventoResultado resultado)
        {
            _db.EventoResultados.Update(resultado);
            await _db.SaveChangesAsync();
        }

        // ── Catálogos ─────────────────────────────────────────────

        public async Task<EstadoEvento?> GetEstadoByNombreAsync(string nombre) =>
            await _db.EstadosEvento.FirstOrDefaultAsync(e => e.Nombre == nombre);
    }

}
