using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("Instructor")]
    public class Instructor
    {
        [Key][Column("id_instructor")] public int Id { get; set; }
        [Column("id_remador")] public int? IdRemador { get; set; }
        [Column("rut")] public string Rut { get; set; } = string.Empty;
        [Column("nombres")] public string Nombres { get; set; } = string.Empty;
        [Column("apellido_paterno")] public string ApellidoPaterno { get; set; } = string.Empty;
        [Column("apellido_materno")] public string? ApellidoMaterno { get; set; }
        [Column("email")] public string Email { get; set; } = string.Empty;
        [Column("telefono")] public string? Telefono { get; set; }
        [Column("activo")] public bool Activo { get; set; } = true;
        [Column("fecha_ingreso")] public DateOnly FechaIngreso { get; set; }
        [Column("fecha_egreso")] public DateOnly? FechaEgreso { get; set; }
        [Column("bio")] public string? Bio { get; set; }
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }

        [ForeignKey("IdRemador")] public Remador? Remador { get; set; }
        [NotMapped] public string NombreCompleto => $"{Nombres} {ApellidoPaterno}".Trim();
    }
}
