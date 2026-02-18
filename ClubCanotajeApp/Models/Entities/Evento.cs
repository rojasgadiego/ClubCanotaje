using ClubCanotajeAPI.Models.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("Evento")]
    public class Evento
    {
        [Key][Column("id_evento")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
        [Column("id_tipo_evento")] public int IdTipoEvento { get; set; }
        [Column("id_estado")] public int IdEstado { get; set; }
        [Column("fecha_inicio")] public DateOnly FechaInicio { get; set; }
        [Column("fecha_fin")] public DateOnly? FechaFin { get; set; }
        [Column("lugar")] public string? Lugar { get; set; }
        [Column("organizador")] public string? Organizador { get; set; }
        [Column("descripcion")] public string? Descripcion { get; set; }
        [Column("url_info")] public string? UrlInfo { get; set; }
        [Column("fecha_limite_inscripcion")] public DateOnly? FechaLimiteInscripcion { get; set; }
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }
        [ForeignKey("IdTipoEvento")] public TipoEvento Tipo { get; set; } = null!;
        [ForeignKey("IdEstado")] public EstadoEvento Estado { get; set; } = null!;
        public ICollection<EventoInscripcion> Inscripciones { get; set; } = [];
    }

    // ── EventoInscripcion ─────────────────────────────────────────

    [Table("EventoInscripcion")]
    public class EventoInscripcion
    {
        [Key][Column("id_inscripcion")] public int Id { get; set; }
        [Column("id_evento")] public int IdEvento { get; set; }
        [Column("id_equipo")] public int? IdEquipo { get; set; }
        [Column("id_remador")] public int? IdRemador { get; set; }
        [Column("id_canoa")] public int? IdCanoa { get; set; }
        [Column("categoria_competencia")] public string? CategoriaCompetencia { get; set; }
        [Column("numero_largada")] public int? NumeroLargada { get; set; }
        [Column("confirmada")] public bool Confirmada { get; set; }
        [Column("fecha_inscripcion")] public DateTime FechaInscripcion { get; set; }
        [Column("observaciones")] public string? Observaciones { get; set; }
        [ForeignKey("IdEvento")] public Evento Evento { get; set; } = null!;
        [ForeignKey("IdEquipo")] public Equipo? Equipo { get; set; }
        [ForeignKey("IdRemador")] public Remador? Remador { get; set; }
        [ForeignKey("IdCanoa")] public Canoa? Canoa { get; set; }
        public EventoResultado? Resultado { get; set; }
    }

    // ── EventoResultado ───────────────────────────────────────────

    [Table("EventoResultado")]
    public class EventoResultado
    {
        [Key][Column("id_resultado")] public int Id { get; set; }
        [Column("id_inscripcion")] public int IdInscripcion { get; set; }
        [Column("posicion_final")] public int? PosicionFinal { get; set; }
        [Column("tiempo_oficial")] public string? TiempoOficial { get; set; }
        [Column("puntos")] public decimal? Puntos { get; set; }
        [Column("descalificado")] public bool Descalificado { get; set; }
        [Column("motivo_desc")] public string? MotivoDesc { get; set; }
        [Column("observaciones")] public string? Observaciones { get; set; }
        [ForeignKey("IdInscripcion")] public EventoInscripcion Inscripcion { get; set; } = null!;
    }
}
