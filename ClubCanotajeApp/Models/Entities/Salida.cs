using ClubCanotajeAPI.Models.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("Salida")]
    public class Salida
    {
        [Key][Column("id_salida")] public int Id { get; set; }
        [Column("id_canoa")] public int IdCanoa { get; set; }
        [Column("fecha_hora_reserva")] public DateTime FechaHoraReserva { get; set; }
        [Column("fecha_hora_programada")] public DateTime FechaHoraProgramada { get; set; }
        [Column("duracion_estimada_min")] public int? DuracionEstimadaMin { get; set; }
        [Column("fecha_hora_salida_real")] public DateTime? FechaHoraSalidaReal { get; set; }
        [Column("fecha_hora_retorno_real")] public DateTime? FechaHoraRetornoReal { get; set; }
        [Column("id_estado")] public int IdEstado { get; set; }
        [Column("id_responsable")] public int IdResponsable { get; set; }
        [Column("zona_recorrido")] public string? ZonaRecorrido { get; set; }
        [Column("condicion_clima")] public string? CondicionClima { get; set; }
        [Column("es_entrenamiento")] public bool EsEntrenamiento { get; set; }
        [Column("id_instructor_asignado")] public int? IdInstructorAsignado { get; set; }
        [Column("id_equipo")] public int? IdEquipo { get; set; }
        [Column("id_motivo_cancelacion")] public int? IdMotivoCancelacion { get; set; }
        [Column("observacion_cancelacion")] public string? ObservacionCancelacion { get; set; }
        [Column("cancelado_por")] public int? CanceladoPor { get; set; }
        [Column("observaciones")] public string? Observaciones { get; set; }
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }
        [Column("creado_por")] public int? CreadoPor { get; set; }
        [ForeignKey("IdCanoa")] public Canoa Canoa { get; set; } = null!;
        [ForeignKey("IdEstado")] public EstadoSalida Estado { get; set; } = null!;
        [ForeignKey("IdResponsable")] public Remador Responsable { get; set; } = null!;
        public ICollection<SalidaParticipante> Participantes { get; set; } = [];
        public ICollection<SalidaImplemento> Implementos { get; set; } = [];

        [NotMapped]
        public int? DuracionRealMin =>
            FechaHoraSalidaReal.HasValue && FechaHoraRetornoReal.HasValue
                ? (int)(FechaHoraRetornoReal.Value - FechaHoraSalidaReal.Value).TotalMinutes
                : null;
    }
}
