using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("EquipoIntegrante")]
    public class EquipoIntegrante
    {
        [Key][Column("id_equipo_integrante")] public int Id { get; set; }
        [Column("id_equipo")] public int IdEquipo { get; set; }
        [Column("id_remador")] public int IdRemador { get; set; }
        [Column("id_rol")] public int? IdRol { get; set; }
        [Column("fecha_ingreso")] public DateOnly FechaIngreso { get; set; }
        [Column("fecha_salida")] public DateOnly? FechaSalida { get; set; }
        [Column("activo")] public bool Activo { get; set; } = true;

        [ForeignKey("IdEquipo")] public Equipo Equipo { get; set; } = null!;
        [ForeignKey("IdRemador")] public Remador Remador { get; set; } = null!;
    }
}
