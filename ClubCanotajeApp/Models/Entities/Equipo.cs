using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubCanotajeAPI.Models.Entities
{
    [Table("Equipo")]
    public class Equipo
    {
        [Key][Column("id_equipo")] public int Id { get; set; }
        [Column("nombre")] public string Nombre { get; set; } = string.Empty;
        [Column("descripcion")] public string? Descripcion { get; set; }
        [Column("id_instructor")] public int? IdInstructor { get; set; }
        [Column("activo")] public bool Activo { get; set; } = true;
        [Column("fecha_creacion")] public DateTime FechaCreacion { get; set; }
        [ForeignKey("IdInstructor")] public Instructor? Instructor { get; set; }
        public ICollection<EquipoIntegrante> Integrantes { get; set; } = [];
    }
}
