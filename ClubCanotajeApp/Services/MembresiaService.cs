using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Dtos.Membresia;
using ClubCanotajeAPI.Models.Dtos.Remador;
using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Repositories.MembresiaRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ClubCanotajeAPI.Services
{
    public class MembresiaService
    {
        private readonly MembresiaRepository _repo;
        private readonly AppDbContext _db;

        public MembresiaService(MembresiaRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<ApiResponse<MembresiaResumenDto>> GetVigenteAsync(int idRemador)
        {
            var m = await _repo.GetVigenteAsync(idRemador);
            if (m is null) return ApiResponse<MembresiaResumenDto>.Fail("No tiene membresía vigente.");
            return ApiResponse<MembresiaResumenDto>.Ok(Map(m));
        }

        public async Task<ApiResponse<MembresiaResumenDto>> CrearAsync(CrearMembresiaDto dto)
        {
            var tipo = await _db.TiposMembresia.FindAsync(dto.IdTipoMembresia);
            if (tipo is null) return ApiResponse<MembresiaResumenDto>.Fail("Tipo de membresía no encontrado.");

            var membresia = new Membresia
            {
                IdRemador = dto.IdRemador,
                IdTipoMembresia = dto.IdTipoMembresia,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaInicio.AddDays(tipo.DuracionDias),
                Activa = true,
                FechaCreacion = DateTime.Now
            };

            await _repo.AddMembresiaAsync(membresia);
            return ApiResponse<MembresiaResumenDto>.Ok(Map(membresia), "Membresía creada correctamente.");
        }

        public async Task<ApiResponse<List<CuotaPendienteDto>>> GetCuotasPendientesAsync()
        {
            var cuotas = await _repo.GetCuotasPendientesAsync();
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var dtos = cuotas.Select(c => new CuotaPendienteDto(
                c.Id,
                c.Membresia.Remador.Rut,
                c.Membresia.Remador.NombreCompleto,
                c.Membresia.Remador.Email,
                c.Membresia.Remador.Telefono,
                c.Periodo,
                c.Monto,
                c.FechaVencimiento,
                c.Estado.Nombre,
                hoy.DayNumber - c.FechaVencimiento.DayNumber
            )).ToList();

            return ApiResponse<List<CuotaPendienteDto>>.Ok(dtos);
        }

        public async Task<ApiResponse> RegistrarPagoAsync(RegistrarPagoDto dto, int idRegistrador)
        {
            var cuota = await _repo.GetCuotaByIdAsync(dto.IdCuota);
            if (cuota is null) return ApiResponse.Fail("Cuota no encontrada.");
            if (cuota.Estado.Nombre == "Pagado") return ApiResponse.Fail("Esta cuota ya está pagada.");

            var pago = new Pago
            {
                IdCuota = dto.IdCuota,
                MontoPagado = dto.MontoPagado,
                FechaPago = DateTime.Now,
                IdMetodoPago = dto.IdMetodoPago,
                Comprobante = dto.Comprobante,
                IdRegistradoPor = idRegistrador,
                Observaciones = dto.Observaciones
            };

            await _repo.AddPagoAsync(pago);

            var idPagado = await _db.EstadosPago
                .Where(e => e.Nombre == "Pagado")
                .Select(e => e.Id)
                .FirstAsync();

            cuota.IdEstado = idPagado;
            await _repo.UpdateCuotaAsync(cuota);

            return ApiResponse.Ok("Pago registrado correctamente.");
        }

        // ── sp_GenerarCuotasMensuales ─────────────────────────────

        public async Task<ApiResponse> GenerarCuotasMensualesAsync(string periodo)
        {
            // Validar formato YYYY-MM
            if (periodo.Length != 7 || periodo[4] != '-')
                return ApiResponse.Fail("Formato de periodo incorrecto. Usar YYYY-MM (ej: 2025-03).");

            var pMensaje = new SqlParameter("@mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
            var pReturnValue = new SqlParameter("@return_value", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            await _db.Database.ExecuteSqlRawAsync(
                @"EXEC @return_value = sp_GenerarCuotasMensuales
                @periodo = @p0,
                @mensaje = @mensaje OUTPUT",
                new SqlParameter("@p0", periodo),
                pMensaje, pReturnValue
            );

            var mensaje = pMensaje.Value?.ToString() ?? "";
            return (int)pReturnValue.Value == 0
                ? ApiResponse.Ok(mensaje)
                : ApiResponse.Fail(mensaje);
        }

        // ── sp_ActualizarCuotasVencidas ───────────────────────────

        public async Task<ApiResponse> ActualizarCuotasVencidasAsync()
        {
            var pMensaje = new SqlParameter("@mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
            var pReturnValue = new SqlParameter("@return_value", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            await _db.Database.ExecuteSqlRawAsync(
                @"EXEC @return_value = sp_ActualizarCuotasVencidas
                @mensaje = @mensaje OUTPUT",
                pMensaje, pReturnValue
            );

            var mensaje = pMensaje.Value?.ToString() ?? "";
            return (int)pReturnValue.Value == 0
                ? ApiResponse.Ok(mensaje)
                : ApiResponse.Fail(mensaje);
        }

        private static MembresiaResumenDto Map(Membresia m)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return new(m.Id, m.TipoMembresia?.Nombre ?? "", m.FechaInicio, m.FechaFin,
                m.EstaVigente, m.FechaFin.DayNumber - hoy.DayNumber);
        }
    }
}
