using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Dtos.Evento;
using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Repositories.EventoRepository;

namespace ClubCanotajeAPI.Services
{
    public class EventoService
    {
        private readonly EventoRepository _repo;

        public EventoService(EventoRepository repo, AppDbContext db)
        {
            _repo = repo;
        }

        // ── Listar eventos ────────────────────────────────────────

        public async Task<ApiResponse<List<EventoListDto>>> GetAllAsync()
        {
            var eventos = await _repo.GetAllAsync();
            return ApiResponse<List<EventoListDto>>.Ok(eventos.Select(MapToList).ToList());
        }

        public async Task<ApiResponse<List<EventoListDto>>> GetProximosAsync()
        {
            var eventos = await _repo.GetProximosAsync();
            return ApiResponse<List<EventoListDto>>.Ok(eventos.Select(MapToList).ToList());
        }

        public async Task<ApiResponse<EventoDetalleDto>> GetByIdAsync(int id)
        {
            var evento = await _repo.GetByIdAsync(id);
            if (evento is null)
                return ApiResponse<EventoDetalleDto>.Fail("Evento no encontrado.");

            return ApiResponse<EventoDetalleDto>.Ok(MapToDetalle(evento));
        }

        // ── Crear evento ──────────────────────────────────────────

        public async Task<ApiResponse<EventoDetalleDto>> CrearAsync(CrearEventoDto dto)
        {
            // Validar fechas
            if (dto.FechaFin.HasValue && dto.FechaFin < dto.FechaInicio)
                return ApiResponse<EventoDetalleDto>.Fail(
                    "La fecha de fin no puede ser anterior a la fecha de inicio.");

            var idEstadoPlanificado = (await _repo.GetEstadoByNombreAsync("Planificado"))!.Id;

            var evento = new Evento
            {
                Nombre = dto.Nombre,
                IdTipoEvento = dto.IdTipoEvento,
                IdEstado = idEstadoPlanificado,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Lugar = dto.Lugar,
                Organizador = dto.Organizador,
                Descripcion = dto.Descripcion,
                UrlInfo = dto.UrlInfo,
                FechaLimiteInscripcion = dto.FechaLimiteInscripcion,
                FechaCreacion = DateTime.Now
            };

            var id = await _repo.AddAsync(evento);
            return await GetByIdAsync(id);
        }

        // ── Actualizar evento ─────────────────────────────────────

        public async Task<ApiResponse<EventoDetalleDto>> ActualizarAsync(
            int id, ActualizarEventoDto dto)
        {
            var evento = await _repo.GetByIdAsync(id);
            if (evento is null)
                return ApiResponse<EventoDetalleDto>.Fail("Evento no encontrado.");

            if (evento.Estado.Nombre == "Finalizado")
                return ApiResponse<EventoDetalleDto>.Fail(
                    "No se puede modificar un evento finalizado.");

            if (dto.Nombre is not null) evento.Nombre = dto.Nombre;
            if (dto.FechaInicio is not null) evento.FechaInicio = dto.FechaInicio.Value;
            if (dto.FechaFin is not null) evento.FechaFin = dto.FechaFin;
            if (dto.Lugar is not null) evento.Lugar = dto.Lugar;
            if (dto.Organizador is not null) evento.Organizador = dto.Organizador;
            if (dto.Descripcion is not null) evento.Descripcion = dto.Descripcion;
            if (dto.UrlInfo is not null) evento.UrlInfo = dto.UrlInfo;
            if (dto.FechaLimiteInscripcion is not null)
                evento.FechaLimiteInscripcion = dto.FechaLimiteInscripcion;

            await _repo.UpdateAsync(evento);
            return await GetByIdAsync(id);
        }

        // ── Cambiar estado del evento ─────────────────────────────

        public async Task<ApiResponse> CambiarEstadoAsync(int id, CambiarEstadoEventoDto dto)
        {
            var evento = await _repo.GetByIdAsync(id);
            if (evento is null)
                return ApiResponse.Fail("Evento no encontrado.");

            evento.IdEstado = dto.IdEstado;
            await _repo.UpdateAsync(evento);

            return ApiResponse.Ok("Estado del evento actualizado correctamente.");
        }

        // ── Abrir inscripciones ───────────────────────────────────

        public async Task<ApiResponse> AbrirInscripcionesAsync(int id)
        {
            var evento = await _repo.GetByIdAsync(id);
            if (evento is null)
                return ApiResponse.Fail("Evento no encontrado.");

            if (evento.Estado.Nombre != "Planificado")
                return ApiResponse.Fail(
                    $"Solo se pueden abrir inscripciones en eventos Planificados. Estado actual: {evento.Estado.Nombre}");

            var idEstado = (await _repo.GetEstadoByNombreAsync("Inscripciones abiertas"))!.Id;
            evento.IdEstado = idEstado;
            await _repo.UpdateAsync(evento);

            return ApiResponse.Ok("Inscripciones abiertas correctamente.");
        }

        // ── Cerrar inscripciones ──────────────────────────────────

        public async Task<ApiResponse> CerrarInscripcionesAsync(int id)
        {
            var evento = await _repo.GetByIdAsync(id);
            if (evento is null)
                return ApiResponse.Fail("Evento no encontrado.");

            if (evento.Estado.Nombre != "Inscripciones abiertas")
                return ApiResponse.Fail("Solo se pueden cerrar las inscripciones abiertas.");

            var idEstado = (await _repo.GetEstadoByNombreAsync("Planificado"))!.Id;
            evento.IdEstado = idEstado;
            await _repo.UpdateAsync(evento);

            return ApiResponse.Ok("Inscripciones cerradas. Ahora puedes iniciar el evento.");
        }

        // ── Iniciar evento ────────────────────────────────────────

        public async Task<ApiResponse> IniciarEventoAsync(int id)
        {
            var evento = await _repo.GetByIdAsync(id);
            if (evento is null)
                return ApiResponse.Fail("Evento no encontrado.");

            if (evento.Estado.Nombre == "En curso")
                return ApiResponse.Fail("El evento ya está en curso.");

            if (evento.Estado.Nombre == "Finalizado")
                return ApiResponse.Fail("El evento ya finalizó.");

            if (evento.Inscripciones.Count == 0)
                return ApiResponse.Fail("No hay participantes inscritos.");

            var idEstado = (await _repo.GetEstadoByNombreAsync("En curso"))!.Id;
            evento.IdEstado = idEstado;
            await _repo.UpdateAsync(evento);

            return ApiResponse.Ok($"Evento iniciado. {evento.Inscripciones.Count} participantes.");
        }

        // ── Finalizar evento ──────────────────────────────────────

        public async Task<ApiResponse> FinalizarEventoAsync(int id)
        {
            var evento = await _repo.GetByIdAsync(id);
            if (evento is null)
                return ApiResponse.Fail("Evento no encontrado.");

            if (evento.Estado.Nombre != "En curso")
                return ApiResponse.Fail("Solo se puede finalizar un evento En curso.");

            var idEstado = (await _repo.GetEstadoByNombreAsync("Finalizado"))!.Id;
            evento.IdEstado = idEstado;
            await _repo.UpdateAsync(evento);

            return ApiResponse.Ok("Evento finalizado correctamente.");
        }

        // ── Inscribir equipo ──────────────────────────────────────

        public async Task<ApiResponse> InscribirEquipoAsync(int idEvento, InscribirEquipoDto dto)
        {
            var evento = await _repo.GetByIdAsync(idEvento);
            if (evento is null)
                return ApiResponse.Fail("Evento no encontrado.");

            if (evento.Estado.Nombre != "Inscripciones abiertas")
                return ApiResponse.Fail("Las inscripciones no están abiertas.");

            if (await _repo.YaInscritoEquipoAsync(idEvento, dto.IdEquipo))
                return ApiResponse.Fail("Este equipo ya está inscrito.");

            var inscripcion = new EventoInscripcion
            {
                IdEvento = idEvento,
                IdEquipo = dto.IdEquipo,
                IdCanoa = dto.IdCanoa,
                CategoriaCompetencia = dto.CategoriaCompetencia,
                Confirmada = false,
                FechaInscripcion = DateTime.Now,
                Observaciones = dto.Observaciones
            };

            await _repo.AddInscripcionAsync(inscripcion);
            return ApiResponse.Ok("Equipo inscrito correctamente.");
        }

        // ── Inscribir remador individual ──────────────────────────

        public async Task<ApiResponse> InscribirRemadorAsync(int idEvento, InscribirRemadorDto dto)
        {
            var evento = await _repo.GetByIdAsync(idEvento);
            if (evento is null)
                return ApiResponse.Fail("Evento no encontrado.");

            if (evento.Estado.Nombre != "Inscripciones abiertas")
                return ApiResponse.Fail("Las inscripciones no están abiertas.");

            if (await _repo.YaInscritoRemadorAsync(idEvento, dto.IdRemador))
                return ApiResponse.Fail("Este remador ya está inscrito.");

            var inscripcion = new EventoInscripcion
            {
                IdEvento = idEvento,
                IdRemador = dto.IdRemador,
                IdCanoa = dto.IdCanoa,
                CategoriaCompetencia = dto.CategoriaCompetencia,
                Confirmada = false,
                FechaInscripcion = DateTime.Now,
                Observaciones = dto.Observaciones
            };

            await _repo.AddInscripcionAsync(inscripcion);
            return ApiResponse.Ok("Remador inscrito correctamente.");
        }

        // ── Confirmar inscripción ─────────────────────────────────

        public async Task<ApiResponse> ConfirmarInscripcionAsync(int idInscripcion)
        {
            var inscripcion = await _repo.GetInscripcionByIdAsync(idInscripcion);
            if (inscripcion is null)
                return ApiResponse.Fail("Inscripción no encontrada.");

            inscripcion.Confirmada = true;
            await _repo.UpdateInscripcionAsync(inscripcion);

            return ApiResponse.Ok("Inscripción confirmada.");
        }

        // ── Asignar números de largada ────────────────────────────

        public async Task<ApiResponse> AsignarNumerosLargadaAsync(
            int idEvento, AsignarNumerosLargadaDto dto)
        {
            var evento = await _repo.GetByIdAsync(idEvento);
            if (evento is null)
                return ApiResponse.Fail("Evento no encontrado.");

            if (evento.Estado.Nombre == "En curso" || evento.Estado.Nombre == "Finalizado")
                return ApiResponse.Fail("No se pueden asignar números en un evento activo o finalizado.");

            // Validar que no haya números duplicados
            var numeros = dto.Asignaciones.Select(a => a.NumeroLargada).ToList();
            if (numeros.Distinct().Count() != numeros.Count)
                return ApiResponse.Fail("Hay números de largada duplicados.");

            foreach (var asig in dto.Asignaciones)
            {
                var inscripcion = await _repo.GetInscripcionByIdAsync(asig.IdInscripcion);
                if (inscripcion is null) continue;

                if (await _repo.ExisteNumeroLargadaAsync(idEvento, asig.NumeroLargada, asig.IdInscripcion))
                    return ApiResponse.Fail($"El número {asig.NumeroLargada} ya está asignado.");

                inscripcion.NumeroLargada = asig.NumeroLargada;
                await _repo.UpdateInscripcionAsync(inscripcion);
            }

            return ApiResponse.Ok("Números de largada asignados correctamente.");
        }

        // ── Registrar resultado ───────────────────────────────────

        public async Task<ApiResponse> RegistrarResultadoAsync(RegistrarResultadoDto dto)
        {
            var inscripcion = await _repo.GetInscripcionByIdAsync(dto.IdInscripcion);
            if (inscripcion is null)
                return ApiResponse.Fail("Inscripción no encontrada.");

            if (inscripcion.Evento.Estado.Nombre != "En curso")
                return ApiResponse.Fail("Solo se pueden registrar resultados en eventos En curso.");

            var resultadoExistente = await _repo.GetResultadoByInscripcionAsync(dto.IdInscripcion);

            if (resultadoExistente is not null)
            {
                // Actualizar resultado existente
                resultadoExistente.PosicionFinal = dto.PosicionFinal;
                resultadoExistente.TiempoOficial = dto.TiempoOficial;
                resultadoExistente.Puntos = dto.Puntos;
                resultadoExistente.Descalificado = dto.Descalificado;
                resultadoExistente.MotivoDesc = dto.MotivoDesc;
                resultadoExistente.Observaciones = dto.Observaciones;

                await _repo.UpdateResultadoAsync(resultadoExistente);
                return ApiResponse.Ok("Resultado actualizado correctamente.");
            }
            else
            {
                // Crear nuevo resultado
                var resultado = new EventoResultado
                {
                    IdInscripcion = dto.IdInscripcion,
                    PosicionFinal = dto.PosicionFinal,
                    TiempoOficial = dto.TiempoOficial,
                    Puntos = dto.Puntos,
                    Descalificado = dto.Descalificado,
                    MotivoDesc = dto.MotivoDesc,
                    Observaciones = dto.Observaciones
                };

                await _repo.AddResultadoAsync(resultado);
                return ApiResponse.Ok("Resultado registrado correctamente.");
            }
        }

        // ── Tabla de resultados ───────────────────────────────────

        public async Task<ApiResponse<TablaResultadosDto>> GetResultadosAsync(int idEvento)
        {
            var evento = await _repo.GetByIdAsync(idEvento);
            if (evento is null)
                return ApiResponse<TablaResultadosDto>.Fail("Evento no encontrado.");

            var resultados = evento.Inscripciones
                .Where(i => i.Resultado is not null)
                .OrderBy(i => i.Resultado!.Descalificado ? int.MaxValue : i.Resultado.PosicionFinal ?? int.MaxValue)
                .Select(i => new ResultadoDto(
                    i.Resultado!.PosicionFinal ?? 0,
                    i.Equipo?.Nombre ?? i.Remador?.NombreCompleto ?? "N/A",
                    i.Equipo is not null ? "Equipo" : "Individual",
                    i.Canoa?.Codigo,
                    i.Resultado.TiempoOficial,
                    i.Resultado.Puntos,
                    i.Resultado.Descalificado,
                    i.Resultado.MotivoDesc
                ))
                .ToList();

            var tabla = new TablaResultadosDto(
                evento.Nombre,
                evento.FechaInicio,
                evento.Lugar ?? "N/A",
                resultados
            );

            return ApiResponse<TablaResultadosDto>.Ok(tabla);
        }

        // ── Mappers ───────────────────────────────────────────────

        private static EventoListDto MapToList(Evento e) => new(
            e.Id, e.Nombre, e.Tipo.Nombre, e.Estado.Nombre,
            e.FechaInicio, e.FechaFin, e.Lugar, e.Inscripciones.Count);

        private static EventoDetalleDto MapToDetalle(Evento e) => new(
            e.Id, e.Nombre, e.Tipo.Nombre, e.Estado.Nombre,
            e.FechaInicio, e.FechaFin, e.Lugar, e.Organizador,
            e.Descripcion, e.UrlInfo, e.FechaLimiteInscripcion,
            e.Inscripciones.Count,
            e.Inscripciones.Select(i => new InscripcionResumenDto(
                i.Id,
                i.Equipo?.Nombre ?? i.Remador?.NombreCompleto ?? "N/A",
                i.Equipo is not null ? "Equipo" : "Individual",
                i.Canoa?.Codigo,
                i.CategoriaCompetencia,
                i.NumeroLargada,
                i.Confirmada,
                i.FechaInscripcion
            )).ToList());
    }
}
