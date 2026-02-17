using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities.Catalogos
{
    [Table("TipoCanoa")]
    public class TipoCanoa
    {
        [Key][Column("id_tipo_canoa")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
        [Column("capacidad_max")] public int CapacidadMax { get; set; }
        [Column("capacidad_min")] public int CapacidadMin { get; set; }
        [Column("descripcion")] public string? Descripcion { get; set; }
    }
}
