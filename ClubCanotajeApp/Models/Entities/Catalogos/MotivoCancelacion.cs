using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities.Catalogos
{
    [Table("MotivoCancelacion")]
    public class MotivoCancelacion
    {
        [Key][Column("id_motivo")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
    }
}
