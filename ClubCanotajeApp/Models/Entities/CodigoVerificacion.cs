using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("CodigoVerificacion")]
    public class CodigoVerificacion
    {
        [Key][Column("id")] public int Id { get; set; }
        [Column("email")] public string Email { get; set; } = string.Empty;
        [Column("codigo")] public string Codigo { get; set; } = string.Empty;
        [Column("tipo")] public string Tipo { get; set; } = string.Empty;
        [Column("expira")] public DateTime Expira { get; set; }
        [Column("usado")] public bool Usado { get; set; }
        [Column("intentos")] public int Intentos { get; set; }
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }

        [NotMapped] public bool EstaVigente => !Usado && Expira > DateTime.Now && Intentos < 3;
    }

    public static class TipoVerificacion
    {
        public const string Registro = "REGISTRO";
        public const string ResetPassword = "RESET_PASSWORD";
    }
}
