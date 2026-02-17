using ClubCanotajeAPI.Models.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("Implemento")]
    public class Implemento
    {
        [Key][Column("id_implemento")] public int Id { get; set; }
        [Column("codigo")] public string Codigo { get; set; } = string.Empty;
        [Column("id_tipo_implemento")] public int IdTipoImplemento { get; set; }
        [Column("marca")] public string? Marca { get; set; }
        [Column("modelo")] public string? Modelo { get; set; }
        [Column("descripcion")] public string? Descripcion { get; set; }
        [Column("id_estado")] public int IdEstado { get; set; }
        [Column("fecha_adquisicion")] public DateOnly? FechaAdquisicion { get; set; }
        [Column("observaciones")] public string? Observaciones { get; set; }
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }
        [Column("fecha_modificacion")] public DateTime? FechaModificacion { get; set; }

        [ForeignKey("IdTipoImplemento")] public TipoImplemento Tipo { get; set; } = null!;
        [ForeignKey("IdEstado")] public EstadoImplemento Estado { get; set; } = null!;
        public ICollection<SalidaImplemento> Salidas { get; set; } = [];
    }
}
