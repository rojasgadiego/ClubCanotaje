using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Models.Entities.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ClubCanotajeAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ── Entidades principales ─────────────────────────────────
        public DbSet<Remador> Remadores => Set<Remador>();
        public DbSet<Instructor> Instructores => Set<Instructor>();
        public DbSet<Canoa> Canoas => Set<Canoa>();
        public DbSet<Salida> Salidas => Set<Salida>();
        public DbSet<SalidaParticipante> SalidaParticipantes => Set<SalidaParticipante>();
        public DbSet<SalidaImplemento> SalidaImplementos => Set<SalidaImplemento>();
        public DbSet<Membresia> Membresias => Set<Membresia>();
        public DbSet<Cuota> Cuotas => Set<Cuota>();
        public DbSet<Pago> Pagos => Set<Pago>();
        public DbSet<Equipo> Equipos => Set<Equipo>();
        public DbSet<EquipoIntegrante> EquipoIntegrantes => Set<EquipoIntegrante>();
        public DbSet<UsuarioSistema> Usuarios => Set<UsuarioSistema>();
        public DbSet<Implemento> Implementos => Set<Implemento>();

        // ── Catálogos ─────────────────────────────────────────────
        public DbSet<TipoImplemento> TiposImplemento => Set<TipoImplemento>();
        public DbSet<EstadoImplemento> EstadosImplemento => Set<EstadoImplemento>();
        public DbSet<EstadoRemador> EstadosRemador => Set<EstadoRemador>();
        public DbSet<CategoriaRemador> CategoriasRemador => Set<CategoriaRemador>();
        public DbSet<TipoCanoa> TiposCanoa => Set<TipoCanoa>();
        public DbSet<EstadoCanoa> EstadosCanoa => Set<EstadoCanoa>();
        public DbSet<EstadoSalida> EstadosSalida => Set<EstadoSalida>();
        public DbSet<RolEnSalida> RolesEnSalida => Set<RolEnSalida>();
        public DbSet<EstadoPago> EstadosPago => Set<EstadoPago>();
        public DbSet<MetodoPago> MetodosPago => Set<MetodoPago>();
        public DbSet<TipoMembresia> TiposMembresia => Set<TipoMembresia>();
        public DbSet<MotivoCancelacion> MotivosCancelacion => Set<MotivoCancelacion>();
        public DbSet<RolSistema> RolesSistema => Set<RolSistema>();

        // ── Verificación de email ─────────────────────────────────
        public DbSet<CodigoVerificacion> CodigosVerificacion => Set<CodigoVerificacion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Índices únicos ────────────────────────────────────
            modelBuilder.Entity<Remador>()
                .HasIndex(r => r.Rut).IsUnique();
            modelBuilder.Entity<Remador>()
                .HasIndex(r => r.Email).IsUnique();
            modelBuilder.Entity<Canoa>()
                .HasIndex(c => c.Codigo).IsUnique();
            modelBuilder.Entity<UsuarioSistema>()
                .HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<SalidaParticipante>()
                .HasIndex(sp => new { sp.IdSalida, sp.IdRemador }).IsUnique();
            modelBuilder.Entity<Cuota>()
                .HasIndex(c => new { c.IdMembresia, c.Periodo }).IsUnique();

            // ── Índices para verificación ────────────────────────
            modelBuilder.Entity<CodigoVerificacion>()
                .HasIndex(cv => new { cv.Email, cv.Tipo, cv.Usado });

            // ── Cascade delete restrictions ──────────────────────
            modelBuilder.Entity<Salida>()
                .HasOne(s => s.Responsable)
                .WithMany()
                .HasForeignKey(s => s.IdResponsable)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Cuota)
                .WithMany(c => c.Pagos)
                .HasForeignKey(p => p.IdCuota)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Propiedades calculadas (no persistidas) ───────────
            modelBuilder.Entity<Remador>().Ignore(r => r.NombreCompleto);
            modelBuilder.Entity<Instructor>().Ignore(i => i.NombreCompleto);
            modelBuilder.Entity<Membresia>().Ignore(m => m.EstaVigente);
            modelBuilder.Entity<Salida>().Ignore(s => s.DuracionRealMin);
            modelBuilder.Entity<CodigoVerificacion>().Ignore(cv => cv.EstaVigente);
        }
    }
}