using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities.Catalogos
{
    [Table("MetodoPago")]
    public class MetodoPago
    {
        [Key][Column("id_metodo")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
    }
}
