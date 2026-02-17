using ClubCanotajeAPI.Models.Dtos.Canoa;
using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Repositories.CanoaRepository;

namespace ClubCanotajeAPI.Services
{
    public class CanoaService
    {
        private readonly CanoaRepository _repo;
        public CanoaService(CanoaRepository repo) => _repo = repo;

        public async Task<ApiResponse<List<CanoaDto>>> GetAllAsync()
        {
            var canoas = await _repo.GetAllAsync();
            return ApiResponse<List<CanoaDto>>.Ok(canoas.Select(Map).ToList());
        }

        public async Task<ApiResponse<List<CanoaDto>>> GetDisponiblesAsync()
        {
            var canoas = await _repo.GetDisponiblesAsync();
            return ApiResponse<List<CanoaDto>>.Ok(canoas.Select(Map).ToList());
        }

        public async Task<ApiResponse<CanoaDto>> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c is null) return ApiResponse<CanoaDto>.Fail("Canoa no encontrada.");
            return ApiResponse<CanoaDto>.Ok(Map(c));
        }

        public async Task<ApiResponse<List<CanoaDisponibleDto>>> GetDisponiblesPorHorarioAsync(DateTime fechaHoraInicio,int duracionMinutos = 120)
        {
            // Validar que el horario esté dentro de la ventana permitida (1 semana adelante)
            var ahora = DateTime.Now;
            var limiteMaximo = ahora.AddDays(7);

            if (fechaHoraInicio < ahora)
                return ApiResponse<List<CanoaDisponibleDto>>.Fail(
                    "No se puede reservar en el pasado.");

            if (fechaHoraInicio > limiteMaximo)
                return ApiResponse<List<CanoaDisponibleDto>>.Fail(
                    "Solo se puede reservar con hasta 7 días de anticipación.");

            var canoas = await _repo.GetDisponiblesPorHorarioAsync(fechaHoraInicio, duracionMinutos);

            var dtos = canoas.Select(c => new CanoaDisponibleDto(
                c.Id,
                c.Codigo,
                c.Nombre,
                c.TipoCanoa.Nombre,
                c.TipoCanoa.CapacidadMin,
                c.TipoCanoa.CapacidadMax,
                c.Color,
                c.UltimaRevision,
                c.ProximaRevision
            )).ToList();

            return ApiResponse<List<CanoaDisponibleDto>>.Ok(dtos);
        }

        private static CanoaDto Map(Canoa c) => new(
            c.Id, c.Codigo, c.Nombre, c.TipoCanoa.Nombre,
            c.TipoCanoa.CapacidadMax, c.TipoCanoa.CapacidadMin,
            c.Estado.Nombre, c.Color, c.ProximaRevision);
    }
}
