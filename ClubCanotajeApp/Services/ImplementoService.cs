using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Dtos.Implemento;
using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Repositories.ImplementoRepository;
using Microsoft.EntityFrameworkCore;

namespace ClubCanotajeAPI.Services
{
    public class ImplementoService
    {
        private readonly ImplementoRepository _repo;
        private readonly AppDbContext _db;

        public ImplementoService(ImplementoRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<ApiResponse<List<ImplementoListDto>>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return ApiResponse<List<ImplementoListDto>>.Ok(items.Select(Map).ToList());
        }

        public async Task<ApiResponse<List<ImplementoListDto>>> GetDisponiblesAsync()
        {
            var items = await _repo.GetDisponiblesAsync();
            return ApiResponse<List<ImplementoListDto>>.Ok(items.Select(Map).ToList());
        }

        public async Task<ApiResponse<List<ImplementoListDto>>> GetDisponiblesPorTipoAsync(int idTipo)
        {
            var items = await _repo.GetDisponiblesPorTipoAsync(idTipo);
            return ApiResponse<List<ImplementoListDto>>.Ok(items.Select(Map).ToList());
        }

        public async Task<ApiResponse<ImplementoDetalleDto>> GetByIdAsync(int id)
        {
            var i = await _repo.GetByIdAsync(id);
            if (i is null) return ApiResponse<ImplementoDetalleDto>.Fail("Implemento no encontrado.");
            return ApiResponse<ImplementoDetalleDto>.Ok(MapDetalle(i));
        }

        public async Task<ApiResponse<ImplementoDetalleDto>> CrearAsync(CrearImplementoDto dto)
        {
            if (await _repo.ExisteCodigoAsync(dto.Codigo))
                return ApiResponse<ImplementoDetalleDto>.Fail($"Ya existe un implemento con el código {dto.Codigo}.");

            var idEstadoDisponible = await _db.EstadosImplemento
                .Where(e => e.Nombre == "Disponible")
                .Select(e => e.Id)
                .FirstAsync();

            var implemento = new Implemento
            {
                Codigo = dto.Codigo,
                IdTipoImplemento = dto.IdTipoImplemento,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Descripcion = dto.Descripcion,
                FechaAdquisicion = dto.FechaAdquisicion,
                Observaciones = dto.Observaciones,
                IdEstado = idEstadoDisponible,
                FechaCreacion = DateTime.Now
            };

            await _repo.AddAsync(implemento);
            return await GetByIdAsync(implemento.Id);
        }

        public async Task<ApiResponse<ImplementoDetalleDto>> ActualizarAsync(int id, ActualizarImplementoDto dto)
        {
            var implemento = await _repo.GetByIdAsync(id);
            if (implemento is null)
                return ApiResponse<ImplementoDetalleDto>.Fail("Implemento no encontrado.");

            if (dto.Marca is not null) implemento.Marca = dto.Marca;
            if (dto.Modelo is not null) implemento.Modelo = dto.Modelo;
            if (dto.Descripcion is not null) implemento.Descripcion = dto.Descripcion;
            if (dto.Observaciones is not null) implemento.Observaciones = dto.Observaciones;

            implemento.FechaModificacion = DateTime.Now;
            await _repo.UpdateAsync(implemento);
            return await GetByIdAsync(id);
        }

        public async Task<ApiResponse> CambiarEstadoAsync(int id, CambiarEstadoImplementoDto dto)
        {
            var implemento = await _repo.GetByIdAsync(id);
            if (implemento is null) return ApiResponse.Fail("Implemento no encontrado.");

            implemento.IdEstado = dto.IdEstado;
            implemento.FechaModificacion = DateTime.Now;
            await _repo.UpdateAsync(implemento);
            return ApiResponse.Ok("Estado actualizado correctamente.");
        }

        // Asignar implemento a una salida activa
        public async Task<ApiResponse> AsignarASalidaAsync(int idSalida, AsignarImplementoDto dto)
        {
            var implemento = await _repo.GetByIdAsync(dto.IdImplemento);
            if (implemento is null)
                return ApiResponse.Fail("Implemento no encontrado.");

            if (implemento.Estado.Nombre != "Disponible")
                return ApiResponse.Fail($"El implemento no está disponible. Estado: {implemento.Estado.Nombre}.");

            // Verificar que la salida existe y está activa
            var salida = await _db.Salidas
                .Include(s => s.Estado)
                .FirstOrDefaultAsync(s => s.Id == idSalida);

            if (salida is null)
                return ApiResponse.Fail("Salida no encontrada.");

            if (salida.Estado.Nombre is not ("Reservada" or "Confirmada" or "En curso"))
                return ApiResponse.Fail($"No se puede asignar implementos a una salida en estado: {salida.Estado.Nombre}.");

            // Verificar que no esté ya asignado a esta salida
            var yaAsignado = await _repo.GetSalidaImplementoAsync(idSalida, dto.IdImplemento);
            if (yaAsignado is not null)
                return ApiResponse.Fail("El implemento ya está asignado a esta salida.");

            // Asignar
            await _repo.AsignarASalidaAsync(new SalidaImplemento
            {
                IdSalida = idSalida,
                IdImplemento = dto.IdImplemento,
                IdRemador = dto.IdRemador,
                Devuelto = false
            });

            // Marcar implemento como En préstamo
            var idEnPrestamo = await _db.EstadosImplemento
                .Where(e => e.Nombre == "En préstamo")
                .Select(e => e.Id)
                .FirstAsync();

            implemento.IdEstado = idEnPrestamo;
            await _repo.UpdateAsync(implemento);

            return ApiResponse.Ok("Implemento asignado correctamente.");
        }

        // Devolver un implemento de una salida
        public async Task<ApiResponse> DevolverAsync(int idSalida, int idImplemento)
        {
            var si = await _repo.GetSalidaImplementoAsync(idSalida, idImplemento);
            if (si is null)
                return ApiResponse.Fail("No se encontró ese implemento en la salida indicada.");

            if (si.Devuelto)
                return ApiResponse.Fail("El implemento ya fue devuelto.");

            si.Devuelto = true;
            await _repo.SaveAsync();

            // Liberar el implemento
            var implemento = await _repo.GetByIdAsync(idImplemento);
            if (implemento is not null)
            {
                var idDisponible = await _db.EstadosImplemento
                    .Where(e => e.Nombre == "Disponible")
                    .Select(e => e.Id)
                    .FirstAsync();

                implemento.IdEstado = idDisponible;
                implemento.FechaModificacion = DateTime.Now;
                await _repo.UpdateAsync(implemento);
            }

            return ApiResponse.Ok("Implemento devuelto correctamente.");
        }

        // ── Mappers ───────────────────────────────────────────────

        private static ImplementoListDto Map(Implemento i) => new(
            i.Id, i.Codigo, i.Tipo.Nombre, i.Marca, i.Modelo,
            i.Estado.Nombre, i.FechaAdquisicion, i.Descripcion);

        private static ImplementoDetalleDto MapDetalle(Implemento i) => new(
            i.Id, i.Codigo, i.Tipo.Nombre, i.Marca, i.Modelo,
            i.Descripcion, i.Estado.Nombre, i.FechaAdquisicion,
            i.Observaciones, i.FechaCreacion);
    }
}
