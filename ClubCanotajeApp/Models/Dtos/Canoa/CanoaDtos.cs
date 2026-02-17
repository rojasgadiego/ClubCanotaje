namespace ClubCanotajeAPI.Models.Dtos.Canoa
{
    public record CanoaDto(
        int Id,
        string Codigo,
        string? Nombre,
        string Tipo,
        int CapacidadMax,
        int CapacidadMin,
        string Estado,
        string? Color,
        DateOnly? ProximaRevision
    );

    public record CanoaDisponibleDto(
        int Id,
        string Codigo,
        string? Nombre,
        string Tipo,
        int CapacidadMin,
        int CapacidadMax,
        string? Color,
        DateOnly? UltimaRevision,
        DateOnly? ProximaRevision
    );
}
