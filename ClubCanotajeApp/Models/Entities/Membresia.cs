using ClubCanotajeAPI.Models.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("Membresia")]
    public class Membresia
    {
        [Key][Column("id_membresia")] public int Id { get; set; }
        [Column("id_remador")] public int IdRemador { get; set; }
        [Column("id_tipo_membresia")] public int IdTipoMembresia { get; set; }
        [Column("fecha_inicio")] public DateOnly FechaInicio { get; set; }
        [Column("fecha_fin")] public DateOnly FechaFin { get; set; }
        [Column("activa")] public bool Activa { get; set; } = true;
        [Column("observaciones")] public string? Observaciones { get; set; }
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }

        [ForeignKey("IdRemador")] public Remador Remador { get; set; } = null!;
        [ForeignKey("IdTipoMembresia")] public TipoMembresia TipoMembresia { get; set; } = null!;
        public ICollection<Cuota> Cuotas { get; set; } = [];

        [NotMapped]
        public bool EstaVigente => Activa
            && DateOnly.FromDateTime(DateTime.Today) >= FechaInicio
            && DateOnly.FromDateTime(DateTime.Today) <= FechaFin;
    }
}
