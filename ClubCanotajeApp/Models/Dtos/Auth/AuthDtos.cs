namespace ClubCanotajeAPI.Models.Dtos.Auth
{
    public record LoginRequest(string Username, string Password);

    public record LoginResponse(
        int Id,
        string Token,
        DateTime Expira,
        string Username,
        string NombreCompleto,
        string Rol
    );

    public record RegistroPublicoRequest(
        // Datos del usuario
        string Username,
        string Password,

        // Datos básicos del remador — se crean junto con el usuario
        string Rut,
        string Nombres,
        string ApellidoPaterno,
        string? ApellidoMaterno,
        DateOnly FechaNacimiento,
        char Genero,
        string Email,
        string? Telefono
    );

    public record RegistroAdminRequest(
        string Username,
        string Password,
        int IdRol        // permite asignar cualquier rol
    );

    public record RegistrarUsuarioResponse(
        int Id,
        string Username,
        string Rol
    );

    public record RolDto(int Id, string Nombre);
}
