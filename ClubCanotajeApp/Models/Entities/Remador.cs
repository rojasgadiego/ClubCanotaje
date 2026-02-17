using ClubCanotajeAPI.Models.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("Remador")]
    public class Remador
    {
        [Key][Column("id_remador")] public int Id { get; set; }
        [Column("rut")] public string Rut { get; set; } = string.Empty;
        [Column("nombres")] public string Nombres { get; set; } = string.Empty;
        [Column("apellido_paterno")] public string ApellidoPaterno { get; set; } = string.Empty;
        [Column("apellido_materno")] public string? ApellidoMaterno { get; set; }
        [Column("fecha_nacimiento")] public DateOnly FechaNacimiento { get; set; }
        [Column("genero")] public char Genero { get; set; }
        [Column("email")] public string Email { get; set; } = string.Empty;
        [Column("telefono")] public string? Telefono { get; set; }
        [Column("telefono_emergencia")] public string? TelefonoEmergencia { get; set; }
        [Column("nombre_contacto_emergencia")] public string? NombreContactoEmergencia { get; set; }
        [Column("direccion")] public string? Direccion { get; set; }
        [Column("foto_url")] public string? FotoUrl { get; set; }
        [Column("cert_medica_vence")] public DateOnly? CertMedicaVence { get; set; }
        [Column("tiene_remo_propio")] public bool TieneRemoPropio { get; set; }
        [Column("tiene_salvavidas_propio")] public bool TieneSalvavidasPropio { get; set; }
        [Column("id_categoria")] public int IdCategoria { get; set; }
        [Column("id_estado")] public int IdEstado { get; set; }
        [Column("fecha_ingreso")] public DateOnly FechaIngreso { get; set; }
        [Column("fecha_retiro")] public DateOnly? FechaRetiro { get; set; }
        [Column("observaciones")] public string? Observaciones { get; set; }
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }
        [Column("fecha_modificacion")] public DateTime? FechaModificacion { get; set; }
        [ForeignKey("IdCategoria")] public CategoriaRemador Categoria { get; set; } = null!;
        [ForeignKey("IdEstado")] public EstadoRemador Estado { get; set; } = null!;
        public ICollection<Membresia> Membresias { get; set; } = [];
        public ICollection<SalidaParticipante> Salidas { get; set; } = [];
        [NotMapped] public string NombreCompleto => $"{Nombres} {ApellidoPaterno}".Trim();
    }
}
