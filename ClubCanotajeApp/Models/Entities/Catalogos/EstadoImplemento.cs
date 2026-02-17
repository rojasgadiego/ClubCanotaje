using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities.Catalogos
{
    [Table("TipoImplemento")]
    public class TipoImplemento
    {
        [Key][Column("id_tipo_implemento")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
        [Column("descripcion")] public string? Descripcion { get; set; }
        public ICollection<Implemento> Implementos { get; set; } = [];
    }

    [Table("EstadoImplemento")]
    public class EstadoImplemento
    {
        [Key][Column("id_estado")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
        [Column("descripcion")] public string? Descripcion { get; set; }
    }
}
