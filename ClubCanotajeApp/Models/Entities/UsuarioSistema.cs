using ClubCanotajeAPI.Models.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("UsuarioSistema")]
    public class UsuarioSistema
    {
        [Key][Column("id_usuario")] public int Id { get; set; }
        [Column("id_remador")] public int? IdRemador { get; set; }
        [Column("id_instructor")] public int? IdInstructor { get; set; }
        [Column("id_rol")] public int IdRol { get; set; }
        [Column("username")] public string Username { get; set; } = string.Empty;
        [Column("password_hash")] public string PasswordHash { get; set; } = string.Empty;
        [Column("activo")] public bool Activo { get; set; } = true;
        [Column("ultimo_acceso")] public DateTime? UltimoAcceso { get; set; }
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }
        [ForeignKey("IdRol")] public RolSistema Rol { get; set; } = null!;
        [ForeignKey("IdRemador")] public Remador? Remador { get; set; }
        [ForeignKey("IdInstructor")] public Instructor? Instructor { get; set; }
        [Column("email_verificado")] public bool EmailVerificado { get; set; }
        [Column("fecha_verificacion")] public DateTime? FechaVerificacion { get; set; }
        
    }
}
