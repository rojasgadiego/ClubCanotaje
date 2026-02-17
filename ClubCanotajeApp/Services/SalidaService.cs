using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Dtos.Salida;
using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Repositories.RemadorRepository;
using ClubCanotajeAPI.Repositories.SalidaRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ClubCanotajeAPI.Services
{
    public class SalidaService
    {
        private readonly SalidaRepository _salidaRepo;
        private readonly RemadorRepository _remadorRepo;
        private readonly AppDbContext _db;

        public SalidaService(
            SalidaRepository salidaRepo,
            RemadorRepository remadorRepo,
            AppDbContext db)
        {
            _salidaRepo = salidaRepo;
            _remadorRepo = remadorRepo;
            _db = db;
        }

        public async Task<ApiResponse<List<SalidaListDto>>> GetProximasAsync()
        {
            var salidas = await _salidaRepo.GetProximasAsync();
            return ApiResponse<List<SalidaListDto>>.Ok(salidas.Select(MapToList).ToList());
        }

        public async Task<ApiResponse<List<SalidaListDto>>> GetHistorialAsync(DateTime? desde, DateTime? hasta)
        {
            var salidas = await _salidaRepo.GetHistorialAsync(desde, hasta);
            return ApiResponse<List<SalidaListDto>>.Ok(salidas.Select(MapToList).ToList());
        }

        public async Task<ApiResponse<SalidaDetalleDto>> GetByIdAsync(int id)
        {
            var s = await _salidaRepo.GetByIdAsync(id);
            if (s is null) return ApiResponse<SalidaDetalleDto>.Fail("Salida no encontrada.");
            return ApiResponse<SalidaDetalleDto>.Ok(MapToDetalle(s));
        }


        public async Task<ApiResponse<SalidaDetalleDto>> CrearReservaAsync(CrearReservaDto dto, int idResponsable)
        {
            var pIdSalida = new SqlParameter("@id_salida", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var pMensaje = new SqlParameter("@mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
            var pReturnValue = new SqlParameter("@return_value", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            await _db.Database.ExecuteSqlRawAsync(
                @"EXEC @return_value = sp_CrearReserva
                @id_canoa               = @p0,
                @fecha_hora_programada  = @p1,
                @duracion_estimada_min  = @p2,
                @id_responsable         = @p3,
                @zona_recorrido         = @p4,
                @es_entrenamiento       = @p5,
                @id_instructor_asignado = @p6,
                @id_equipo              = @p7,
                @id_salida              = @id_salida  OUTPUT,
                @mensaje                = @mensaje    OUTPUT",
                new SqlParameter("@p0", dto.IdCanoa),
                new SqlParameter("@p1", dto.FechaHoraProgramada),
                new SqlParameter("@p2", dto.DuracionEstimadaMin),
                new SqlParameter("@p3", idResponsable),
                new SqlParameter("@p4", (object?)dto.ZonaRecorrido ?? DBNull.Value),
                new SqlParameter("@p5", dto.EsEntrenamiento),
                new SqlParameter("@p6", (object?)dto.IdInstructorAsignado ?? DBNull.Value),
                new SqlParameter("@p7", (object?)dto.IdEquipo ?? DBNull.Value),
                pIdSalida, pMensaje, pReturnValue
            );

            if ((int)pReturnValue.Value != 0)
                return ApiResponse<SalidaDetalleDto>.Fail(pMensaje.Value?.ToString() ?? "Error al crear reserva.");

            return await GetByIdAsync((int)pIdSalida.Value);
        }


        public async Task<ApiResponse> AgregarParticipanteAsync(int idSalida, AgregarParticipanteDto dto)
        {
            var salida = await _salidaRepo.GetByIdAsync(idSalida);
            if (salida is null) return ApiResponse.Fail("Salida no encontrada.");

            if (salida.Estado.Nombre is not ("Reservada" or "Confirmada"))
                return ApiResponse.Fail("Solo se pueden agregar participantes a salidas Reservadas o Confirmadas.");

            if (salida.Participantes.Any(p => p.IdRemador == dto.IdRemador))
                return ApiResponse.Fail("El remador ya está inscrito en esta salida.");

            if (salida.Participantes.Count >= salida.Canoa.TipoCanoa.CapacidadMax)
                return ApiResponse.Fail($"Capacidad máxima de {salida.Canoa.TipoCanoa.CapacidadMax} remadores alcanzada.");

            if (!await _remadorRepo.TieneMembresiaVigenteAsync(dto.IdRemador))
                return ApiResponse.Fail("El remador no tiene membresía vigente.");

            await _salidaRepo.AddParticipanteAsync(new SalidaParticipante
            {
                IdSalida = idSalida,
                IdRemador = dto.IdRemador,
                IdRol = dto.IdRol,
                UsaRemoClub = dto.UsaRemoClub,
                UsaSalvavidasClub = dto.UsaSalvavidasClub,
                ConfirmoAsistencia = false
            });

            return ApiResponse.Ok("Participante agregado correctamente.");
        }


        public async Task<ApiResponse> IniciarAsync(int idSalida, IniciarSalidaDto dto)
        {
            var pMensaje = new SqlParameter("@mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
            var pReturnValue = new SqlParameter("@return_value", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            await _db.Database.ExecuteSqlRawAsync(
                @"EXEC @return_value = sp_IniciarSalida
                @id_salida       = @p0,
                @condicion_clima = @p1,
                @mensaje         = @mensaje OUTPUT",
                new SqlParameter("@p0", idSalida),
                new SqlParameter("@p1", (object?)dto.CondicionClima ?? DBNull.Value),
                pMensaje, pReturnValue
            );

            var mensaje = pMensaje.Value?.ToString() ?? "";
            return (int)pReturnValue.Value == 0
                ? ApiResponse.Ok(mensaje)
                : ApiResponse.Fail(mensaje);
        }


        public async Task<ApiResponse> FinalizarAsync(int idSalida, FinalizarSalidaDto dto)
        {
            var pMensaje = new SqlParameter("@mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
            var pReturnValue = new SqlParameter("@return_value", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            await _db.Database.ExecuteSqlRawAsync(
                @"EXEC @return_value = sp_FinalizarSalida
                @id_salida     = @p0,
                @observaciones = @p1,
                @mensaje       = @mensaje OUTPUT",
                new SqlParameter("@p0", idSalida),
                new SqlParameter("@p1", (object?)dto.Observaciones ?? DBNull.Value),
                pMensaje, pReturnValue
            );

            var mensaje = pMensaje.Value?.ToString() ?? "";
            return (int)pReturnValue.Value == 0
                ? ApiResponse.Ok(mensaje)
                : ApiResponse.Fail(mensaje);
        }


        public async Task<ApiResponse> CancelarAsync(int idSalida, CancelarReservaDto dto, int canceladoPor)
        {
            var pMensaje = new SqlParameter("@mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
            var pReturnValue = new SqlParameter("@return_value", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            await _db.Database.ExecuteSqlRawAsync(
                @"EXEC @return_value = sp_CancelarReserva
                @id_salida             = @p0,
                @id_motivo_cancelacion = @p1,
                @observacion           = @p2,
                @cancelado_por         = @p3,
                @mensaje               = @mensaje OUTPUT",
                new SqlParameter("@p0", idSalida),
                new SqlParameter("@p1", dto.IdMotivoCancelacion),
                new SqlParameter("@p2", (object?)dto.Observacion ?? DBNull.Value),
                new SqlParameter("@p3", canceladoPor),
                pMensaje, pReturnValue
            );

            var mensaje = pMensaje.Value?.ToString() ?? "";
            return (int)pReturnValue.Value == 0
                ? ApiResponse.Ok(mensaje)
                : ApiResponse.Fail(mensaje);
        }

        private static SalidaListDto MapToList(Salida s) => new(
            s.Id, s.FechaHoraProgramada,
            s.Canoa.Codigo, s.Canoa.Nombre, s.Canoa.TipoCanoa.Nombre,
            s.Estado.Nombre, s.Responsable.NombreCompleto,
            s.Participantes.Count,
            s.Canoa.TipoCanoa.CapacidadMin, s.Canoa.TipoCanoa.CapacidadMax,
            s.ZonaRecorrido);

        private static SalidaDetalleDto MapToDetalle(Salida s) => new(
            s.Id, s.FechaHoraReserva, s.FechaHoraProgramada, s.DuracionEstimadaMin,
            s.FechaHoraSalidaReal, s.FechaHoraRetornoReal, s.DuracionRealMin,
            s.Canoa.Codigo, s.Canoa.Nombre, s.Canoa.TipoCanoa.Nombre,
            s.Estado.Nombre, s.Responsable.NombreCompleto,
            s.ZonaRecorrido, s.CondicionClima, s.EsEntrenamiento,
            s.Participantes.Select(p => new ParticipanteDto(
                p.IdRemador, p.Remador.NombreCompleto,
                p.Rol?.Nombre, p.UsaRemoClub, p.UsaSalvavidasClub, p.ConfirmoAsistencia)));
    }
}
