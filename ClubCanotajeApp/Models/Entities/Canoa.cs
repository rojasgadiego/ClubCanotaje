using ClubCanotajeAPI.Models.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("Canoa")]
    public class Canoa
    {
        [Key][Column("id_canoa")] public int Id { get; set; }
        [Column("codigo")] public string Codigo { get; set; } = string.Empty;
        [Column("nombre")] public string? Nombre { get; set; }
        [Column("id_tipo_canoa")] public int IdTipoCanoa { get; set; }
        [Column("marca")] public string? Marca { get; set; }
        [Column("modelo")] public string? Modelo { get; set; }
        [Column("anio_fabricacion")] public int? AnioFabricacion { get; set; }
        [Column("color")] public string? Color { get; set; }
        [Column("numero_serie")] public string? NumeroSerie { get; set; }
        [Column("id_estado")] public int IdEstado { get; set; }
        [Column("fecha_adquisicion")] public DateOnly? FechaAdquisicion { get; set; }
        [Column("ultima_revision")] public DateOnly? UltimaRevision { get; set; }
        [Column("proxima_revision")] public DateOnly? ProximaRevision { get; set; }
        [Column("observaciones")] public string? Observaciones { get; set; }
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }
        [Column("fecha_modificacion")] public DateTime? FechaModificacion { get; set; }

        [ForeignKey("IdTipoCanoa")] public TipoCanoa TipoCanoa { get; set; } = null!;
        [ForeignKey("IdEstado")] public EstadoCanoa Estado { get; set; } = null!;
        public ICollection<Salida> Salidas { get; set; } = [];
    }
}
