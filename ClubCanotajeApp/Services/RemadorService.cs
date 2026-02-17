using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Dtos.Remador;
using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Repositories.RemadorRepository;

namespace ClubCanotajeAPI.Services
{
    public class RemadorService
    {
        private readonly RemadorRepository _repo;

        public RemadorService(RemadorRepository repo) => _repo = repo;

        public async Task<ApiResponse<List<RemadorListDto>>> GetAllAsync()
        {
            var remadores = await _repo.GetAllAsync();
            var dtos = remadores.Select(r => MapToList(r)).ToList();
            return ApiResponse<List<RemadorListDto>>.Ok(dtos);
        }

        public async Task<ApiResponse<RemadorDetalleDto>> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r is null) return ApiResponse<RemadorDetalleDto>.Fail("Remador no encontrado.");
            return ApiResponse<RemadorDetalleDto>.Ok(MapToDetalle(r));
        }

        public async Task<ApiResponse<RemadorDetalleDto>> CrearAsync(CrearRemadorDto dto)
        {
            if (await _repo.ExisteRutAsync(dto.Rut))
                return ApiResponse<RemadorDetalleDto>.Fail("Ya existe un remador con ese RUT.");
            if (await _repo.ExisteEmailAsync(dto.Email))
                return ApiResponse<RemadorDetalleDto>.Fail("Ya existe un remador con ese email.");

            var remador = new Remador
            {
                Rut = dto.Rut,
                Nombres = dto.Nombres,
                ApellidoPaterno = dto.ApellidoPaterno,
                ApellidoMaterno = dto.ApellidoMaterno,
                FechaNacimiento = dto.FechaNacimiento,
                Genero = dto.Genero,
                Email = dto.Email,
                Telefono = dto.Telefono,
                TelefonoEmergencia = dto.TelefonoEmergencia,
                NombreContactoEmergencia = dto.NombreContactoEmergencia,
                TieneRemoPropio = dto.TieneRemoPropio,
                TieneSalvavidasPropio = dto.TieneSalvavidasPropio,
                IdCategoria = dto.IdCategoria,
                IdEstado = 1, // Activo por defecto
                FechaIngreso = DateOnly.FromDateTime(DateTime.Today),
                FechaCreacion = DateTime.Now
            };

            await _repo.AddAsync(remador);
            return await GetByIdAsync(remador.Id);
        }

        public async Task<ApiResponse<RemadorDetalleDto>> ActualizarAsync(int id, ActualizarRemadorDto dto)
        {
            var remador = await _repo.GetByIdAsync(id);
            if (remador is null) return ApiResponse<RemadorDetalleDto>.Fail("Remador no encontrado.");

            if (dto.Telefono is not null) remador.Telefono = dto.Telefono;
            if (dto.TelefonoEmergencia is not null) remador.TelefonoEmergencia = dto.TelefonoEmergencia;
            if (dto.NombreContactoEmergencia is not null) remador.NombreContactoEmergencia = dto.NombreContactoEmergencia;
            if (dto.Direccion is not null) remador.Direccion = dto.Direccion;
            if (dto.FotoUrl is not null) remador.FotoUrl = dto.FotoUrl;
            if (dto.CertMedicaVence is not null) remador.CertMedicaVence = dto.CertMedicaVence;
            if (dto.TieneRemoPropio is not null) remador.TieneRemoPropio = dto.TieneRemoPropio.Value;
            if (dto.TieneSalvavidasPropio is not null) remador.TieneSalvavidasPropio = dto.TieneSalvavidasPropio.Value;
            if (dto.IdCategoria is not null) remador.IdCategoria = dto.IdCategoria.Value;

            remador.FechaModificacion = DateTime.Now;
            await _repo.UpdateAsync(remador);
            return await GetByIdAsync(id);
        }

        // ── Mappers privados ──────────────────────────────────────

        private static RemadorListDto MapToList(Remador r)
        {
            var membresia = r.Membresias.FirstOrDefault();
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var estMemb = membresia is null ? "Sin membresía"
                           : hoy > membresia.FechaFin ? "Vencida"
                           : hoy < membresia.FechaInicio ? "Futura"
                           : "Vigente";
            var diasMemb = membresia is not null
                ? (int?)membresia.FechaFin.DayNumber - hoy.DayNumber : null;
            var estCert = r.CertMedicaVence is null ? "Sin certificado"
                           : r.CertMedicaVence < hoy ? "Vencida"
                           : r.CertMedicaVence < hoy.AddDays(30) ? "Por vencer"
                           : "Vigente";

            return new RemadorListDto(r.Id, r.Rut, r.NombreCompleto, r.Email, r.Telefono,
                r.Categoria.Nombre, r.Estado.Nombre, estMemb, diasMemb, estCert);
        }

        private static RemadorDetalleDto MapToDetalle(Remador r)
        {
            var membresia = r.Membresias.FirstOrDefault();
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            MembresiaResumenDto? membDto = membresia is null ? null : new(
                membresia.Id,
                membresia.TipoMembresia.Nombre,
                membresia.FechaInicio,
                membresia.FechaFin,
                membresia.EstaVigente,
                membresia.FechaFin.DayNumber - hoy.DayNumber
            );

            return new RemadorDetalleDto(r.Id, r.Rut, r.Nombres, r.ApellidoPaterno, r.ApellidoMaterno,
                r.FechaNacimiento, r.Genero, r.Email, r.Telefono, r.TelefonoEmergencia,
                r.NombreContactoEmergencia, r.Direccion, r.FotoUrl, r.CertMedicaVence,
                r.TieneRemoPropio, r.TieneSalvavidasPropio,
                r.Categoria.Nombre, r.Estado.Nombre, r.FechaIngreso, membDto);
        }
    }
}
