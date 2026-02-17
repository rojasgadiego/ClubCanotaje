using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities.Catalogos
{
    [Table("TipoMembresia")]
    public class TipoMembresia
    {
        [Key][Column("id_tipo_membresia")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
        [Column("duracion_dias")] public int DuracionDias { get; set; }
        [Column("precio")] public decimal Precio { get; set; }
        [Column("descripcion")] public string? Descripcion { get; set; }
        [Column("activo")] public bool Activo { get; set; } = true;
    }
}
