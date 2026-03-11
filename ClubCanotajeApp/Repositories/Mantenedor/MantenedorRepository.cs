using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Entities.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ClubCanotajeAPI.Repositories.MantenedorRepository
{
    public class MantenedorRepository
    {
        private readonly AppDbContext _db;
        public MantenedorRepository(AppDbContext db) => _db = db;

        public async Task<List<EstadoCanoa>> GetEstadoCanoasAsync() =>
            await _db.EstadosCanoa.AsNoTracking()
            .Select(e => new EstadoCanoa { Nombre = e.Nombre })
            .ToListAsync();

        public async Task<List<TipoCanoa>> GetTipoCanoasAsync() =>
            await _db.TiposCanoa.AsNoTracking()
            .Select(t => new TipoCanoa { Nombre = t.Nombre, CapacidadMax = t.CapacidadMax, CapacidadMin = t.CapacidadMin, Descripcion = t.Descripcion })
            .ToListAsync();

        public async Task<List<MotivoCancelacion>> GetMotivoCancelacionAsync() =>
            await _db.MotivosCancelacion.AsNoTracking()
            .Select(m => new MotivoCancelacion { Id = m.Id, Nombre = m.Nombre })
            .ToListAsync();

        public async Task<List<RolEnSalida>> GetRolEnSalidasAsync() =>
            await _db.RolesEnSalida.AsNoTracking()
            .Select(r => new RolEnSalida { Id = r.Id, Nombre = r.Nombre })
            .ToListAsync();

        public async Task<List<TipoMembresia>> GetTipoMembresiasAsync() =>
            await _db.TiposMembresia.AsNoTracking()
            .Select(t => new TipoMembresia { Id = t.Id, Nombre = t.Nombre, Descripcion = t.Descripcion, DuracionDias = t.DuracionDias, Precio = t.Precio })
            .ToListAsync();

    }
}