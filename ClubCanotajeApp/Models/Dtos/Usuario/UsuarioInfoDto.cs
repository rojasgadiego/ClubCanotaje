namespace ClubCanotajeAPI.Models.Dtos.Usuario
{
    public class UsuarioInfoDto
    {
        // Usuario
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EmailVerificado { get; set; }

        // Remador (nullable, solo si tiene)
        public RemadorInfoDto? Remador { get; set; }
    }

    public class RemadorInfoDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Rut { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public char Genero { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateOnly FechaIngreso { get; set; }
        public string? FotoUrl { get; set; }
        public DateOnly? CertMedicaVence { get; set; }
        public bool TieneRemoPropio { get; set; }
        public bool TieneSalvavidasPropio { get; set; }
        public string? Observaciones { get; set; }

        // Contacto emergencia
        public string? NombreContactoEmergencia { get; set; }
        public string? TelefonoEmergencia { get; set; }
    }
}