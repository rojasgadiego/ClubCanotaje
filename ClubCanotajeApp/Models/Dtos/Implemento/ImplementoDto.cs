namespace ClubCanotajeAPI.Models.Dtos.Implemento
{
    public record ImplementoListDto(
        int Id,
        string Codigo,
        string Tipo,
        string? Marca,
        string? Modelo,
        string Estado,
        DateOnly? FechaAdquisicion,
        string? Descripcion
    );

    public record ImplementoDetalleDto(
        int Id,
        string Codigo,
        string Tipo,
        string? Marca,
        string? Modelo,
        string? Descripcion,
        string Estado,
        DateOnly? FechaAdquisicion,
        string? Observaciones,
        DateTime FechaCreacion
    );

    public record CrearImplementoDto(
        string Codigo,
        int IdTipoImplemento,
        string? Marca,
        string? Modelo,
        string? Descripcion,
        DateOnly? FechaAdquisicion,
        string? Observaciones
    );

    public record ActualizarImplementoDto(
        string? Marca,
        string? Modelo,
        string? Descripcion,
        string? Observaciones
    );

    public record CambiarEstadoImplementoDto(int IdEstado);

    // Para asignar un implemento a una salida
    public record AsignarImplementoDto(
        int IdImplemento,
        int IdRemador       // a qué remador se le asigna dentro de la salida
    );
}
