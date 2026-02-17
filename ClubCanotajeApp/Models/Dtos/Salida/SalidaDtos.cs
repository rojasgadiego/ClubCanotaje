namespace ClubCanotajeAPI.Models.Dtos.Salida
{
    public record CrearReservaDto(
        int IdCanoa,
        DateTime FechaHoraProgramada,
        int DuracionEstimadaMin,
        string? ZonaRecorrido,
        bool EsEntrenamiento,
        int? IdInstructorAsignado,
        int? IdEquipo
    );

    public record SalidaListDto(
        int Id,
        DateTime FechaHoraProgramada,
        string CodigoCanoa,
        string? NombreCanoa,
        string TipoCanoa,
        string Estado,
        string Responsable,
        int Participantes,
        int CapacidadMin,
        int CapacidadMax,
        string? ZonaRecorrido
    );

    public record SalidaDetalleDto(
        int Id,
        DateTime FechaHoraReserva,
        DateTime FechaHoraProgramada,
        int? DuracionEstimadaMin,
        DateTime? FechaHoraSalidaReal,
        DateTime? FechaHoraRetornoReal,
        int? DuracionRealMin,
        string CodigoCanoa,
        string? NombreCanoa,
        string TipoCanoa,
        string Estado,
        string Responsable,
        string? ZonaRecorrido,
        string? CondicionClima,
        bool EsEntrenamiento,
        IEnumerable<ParticipanteDto> Participantes
    );

    public record ParticipanteDto(
        int IdRemador,
        string NombreCompleto,
        string? Rol,
        bool UsaRemoClub,
        bool UsaSalvavidasClub,
        bool ConfirmoAsistencia
    );

    public record AgregarParticipanteDto(int IdRemador, int? IdRol, bool UsaRemoClub, bool UsaSalvavidasClub);
    public record IniciarSalidaDto(string? CondicionClima);
    public record FinalizarSalidaDto(string? Observaciones);
    public record CancelarReservaDto(int IdMotivoCancelacion, string? Observacion);
}
