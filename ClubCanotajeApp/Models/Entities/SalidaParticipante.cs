using ClubCanotajeAPI.Models.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("SalidaParticipante")]
    public class SalidaParticipante
    {
        [Key][Column("id_participante")] public int Id { get; set; }
        [Column("id_salida")] public int IdSalida { get; set; }
        [Column("id_remador")] public int IdRemador { get; set; }
        [Column("id_rol")] public int? IdRol { get; set; }
        [Column("usa_remo_club")] public bool UsaRemoClub { get; set; } = true;
        [Column("usa_salvavidas_club")] public bool UsaSalvavidasClub { get; set; } = true;
        [Column("confirmo_asistencia")] public bool ConfirmoAsistencia { get; set; }

        [ForeignKey("IdSalida")] public Salida Salida { get; set; } = null!;
        [ForeignKey("IdRemador")] public Remador Remador { get; set; } = null!;
        [ForeignKey("IdRol")] public RolEnSalida? Rol { get; set; }
    }
}
