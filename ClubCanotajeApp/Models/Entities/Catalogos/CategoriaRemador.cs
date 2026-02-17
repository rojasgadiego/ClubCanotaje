using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities.Catalogos
{
    [Table("CategoriaRemador")]
    public class CategoriaRemador
    {
        [Key][Column("id_categoria")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
        [Column("edad_min")] public int? EdadMin { get; set; }
        [Column("edad_max")] public int? EdadMax { get; set; }
        [Column("descripcion")] public string? Descripcion { get; set; }
        public ICollection<Remador> Remadores { get; set; } = [];
    }
}
