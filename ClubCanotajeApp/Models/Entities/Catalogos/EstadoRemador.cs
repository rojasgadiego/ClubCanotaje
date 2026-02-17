using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities.Catalogos
{
    [Table("EstadoRemador")]
    public class EstadoRemador
    {
        [Key][Column("id_estado")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
        [Column("descripcion")] public string? Descripcion { get; set; }
    }
}
