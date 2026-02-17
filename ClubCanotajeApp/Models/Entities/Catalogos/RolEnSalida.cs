using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities.Catalogos
{
    [Table("RolEnSalida")]
    public class RolEnSalida
    {
        [Key][Column("id_rol")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
        [Column("descripcion")] public string? Descripcion { get; set; }
    }
}
