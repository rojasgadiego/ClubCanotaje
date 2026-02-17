using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("SalidaImplemento")]
    public class SalidaImplemento
    {
        [Key][Column("id_salida_implemento")] public int Id { get; set; }
        [Column("id_salida")] public int IdSalida { get; set; }
        [Column("id_implemento")] public int IdImplemento { get; set; }
        [Column("id_remador")] public int IdRemador { get; set; }
        [Column("devuelto")] public bool Devuelto { get; set; }
        [Column("observaciones")] public string? Observaciones { get; set; }

        [ForeignKey("IdSalida")] public Salida Salida { get; set; } = null!;
    }
}
