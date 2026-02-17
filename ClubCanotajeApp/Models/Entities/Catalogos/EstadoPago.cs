using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities.Catalogos
{
    [Table("EstadoPago")]
    public class EstadoPago
    {
        [Key][Column("id_estado")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
    }
}
