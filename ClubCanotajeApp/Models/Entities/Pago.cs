
using ClubCanotajeAPI.Models.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("Pago")]
    public class Pago
    {
        [Key][Column("id_pago")] public int Id { get; set; }
        [Column("id_cuota")] public int IdCuota { get; set; }
        [Column("monto_pagado")] public decimal MontoPagado { get; set; }
        [Column("fecha_pago")] public DateTime FechaPago { get; set; }
        [Column("id_metodo_pago")] public int IdMetodoPago { get; set; }
        [Column("comprobante")] public string? Comprobante { get; set; }
        [Column("id_registrado_por")] public int? IdRegistradoPor { get; set; }
        [Column("observaciones")] public string? Observaciones { get; set; }
        [ForeignKey("IdCuota")] public Cuota Cuota { get; set; } = null!;
        [ForeignKey("IdMetodoPago")] public MetodoPago MetodoPago { get; set; } = null!;
    }
}
