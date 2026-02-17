namespace ClubCanotajeAPI.Models.Dtos.Membresia
{
    public record CrearMembresiaDto(int IdRemador, int IdTipoMembresia, DateOnly FechaInicio);

    public record CuotaPendienteDto(
        int IdCuota,
        string Rut,
        string NombreCompleto,
        string Email,
        string? Telefono,
        string Periodo,
        decimal Monto,
        DateOnly FechaVencimiento,
        string EstadoPago,
        int DiasVencida
    );

    public record RegistrarPagoDto(
        int IdCuota,
        decimal MontoPagado,
        int IdMetodoPago,
        string? Comprobante,
        string? Observaciones
    );
}
