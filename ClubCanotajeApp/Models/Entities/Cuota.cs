using ClubCanotajeAPI.Models.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("Cuota")]
    public class Cuota
    {
        [Key][Column("id_cuota")] public int Id { get; set; }
        [Column("id_membresia")] public int IdMembresia { get; set; }
        [Column("periodo")] public string Periodo { get; set; } = string.Empty;
        [Column("monto")] public decimal Monto { get; set; }
        [Column("fecha_vencimiento")] public DateOnly FechaVencimiento { get; set; }
        [Column("id_estado")] public int IdEstado { get; set; }
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }
        [ForeignKey("IdMembresia")] public Membresia Membresia { get; set; } = null!;
        [ForeignKey("IdEstado")] public EstadoPago Estado { get; set; } = null!;
        public ICollection<Pago> Pagos { get; set; } = [];
    }
}
