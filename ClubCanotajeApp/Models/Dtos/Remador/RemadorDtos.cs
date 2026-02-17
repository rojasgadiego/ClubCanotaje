namespace ClubCanotajeAPI.Models.Dtos.Remador
{
    public record RemadorListDto(
        int Id,
        string Rut,
        string NombreCompleto,
        string Email,
        string? Telefono,
        string Categoria,
        string Estado,
        string EstadoMembresia,
        int? DiasParaVencer,
        string EstadoCertMedica
    );

    public record RemadorDetalleDto(
        int Id,
        string Rut,
        string Nombres,
        string ApellidoPaterno,
        string? ApellidoMaterno,
        DateOnly FechaNacimiento,
        char Genero,
        string Email,
        string? Telefono,
        string? TelefonoEmergencia,
        string? NombreContactoEmergencia,
        string? Direccion,
        string? FotoUrl,
        DateOnly? CertMedicaVence,
        bool TieneRemoPropio,
        bool TieneSalvavidasPropio,
        string Categoria,
        string Estado,
        DateOnly FechaIngreso,
        MembresiaResumenDto? MembresiaVigente
    );

    public record MembresiaResumenDto(
        int Id,
        string Tipo,
        DateOnly FechaInicio,
        DateOnly FechaFin,
        bool EstaVigente,
        int DiasRestantes
    );

    public record CrearRemadorDto(
        string Rut,
        string Nombres,
        string ApellidoPaterno,
        string? ApellidoMaterno,
        DateOnly FechaNacimiento,
        char Genero,
        string Email,
        string? Telefono,
        string? TelefonoEmergencia,
        string? NombreContactoEmergencia,
        bool TieneRemoPropio,
        bool TieneSalvavidasPropio,
        int IdCategoria
    );

    public record ActualizarRemadorDto(
        string? Telefono,
        string? TelefonoEmergencia,
        string? NombreContactoEmergencia,
        string? Direccion,
        string? FotoUrl,
        DateOnly? CertMedicaVence,
        bool? TieneRemoPropio,
        bool? TieneSalvavidasPropio,
        int? IdCategoria
    );
}
