namespace ClubCanotajeAPI.Models.Dtos.Evento
{
    public record EventoListDto(
    int Id,
    string Nombre,
    string Tipo,
    string Estado,
    DateOnly FechaInicio,
    DateOnly? FechaFin,
    string? Lugar,
    int TotalInscritos
);

    public record EventoDetalleDto(
        int Id,
        string Nombre,
        string Tipo,
        string Estado,
        DateOnly FechaInicio,
        DateOnly? FechaFin,
        string? Lugar,
        string? Organizador,
        string? Descripcion,
        string? UrlInfo,
        DateOnly? FechaLimiteInscripcion,
        int TotalInscritos,
        List<InscripcionResumenDto> Inscripciones
    );

    public record CrearEventoDto(
        string Nombre,
        int IdTipoEvento,
        DateOnly FechaInicio,
        DateOnly? FechaFin,
        string? Lugar,
        string? Organizador,
        string? Descripcion,
        string? UrlInfo,
        DateOnly? FechaLimiteInscripcion
    );

    public record ActualizarEventoDto(
        string? Nombre,
        DateOnly? FechaInicio,
        DateOnly? FechaFin,
        string? Lugar,
        string? Organizador,
        string? Descripcion,
        string? UrlInfo,
        DateOnly? FechaLimiteInscripcion
    );

    public record CambiarEstadoEventoDto(int IdEstado);

    // ── Inscripción ───────────────────────────────────────────────

    public record InscribirEquipoDto(
        int IdEquipo,
        int? IdCanoa,
        string? CategoriaCompetencia,
        string? Observaciones
    );

    public record InscribirRemadorDto(
        int IdRemador,
        int? IdCanoa,
        string? CategoriaCompetencia,
        string? Observaciones
    );

    public record InscripcionResumenDto(
        int Id,
        string Participante,     // Nombre del equipo o remador
        string Tipo,             // "Equipo" o "Individual"
        string? Canoa,
        string? Categoria,
        int? NumeroLargada,
        bool Confirmada,
        DateTime FechaInscripcion
    );

    public record ConfirmarInscripcionDto(int IdInscripcion);

    public record AsignarNumerosLargadaDto(
        List<AsignacionLargada> Asignaciones
    );

    public record AsignacionLargada(int IdInscripcion, int NumeroLargada);

    // ── Resultados ────────────────────────────────────────────────

    public record RegistrarResultadoDto(
        int IdInscripcion,
        int? PosicionFinal,
        string? TiempoOficial,    // Formato: "00:12:34.56"
        decimal? Puntos,
        bool Descalificado,
        string? MotivoDesc,
        string? Observaciones
    );

    public record ResultadoDto(
        int Posicion,
        string Participante,
        string Tipo,
        string? Canoa,
        string? TiempoOficial,
        decimal? Puntos,
        bool Descalificado,
        string? MotivoDesc
    );

    public record TablaResultadosDto(
        string Evento,
        DateOnly Fecha,
        string Lugar,
        List<ResultadoDto> Resultados
    );
}
