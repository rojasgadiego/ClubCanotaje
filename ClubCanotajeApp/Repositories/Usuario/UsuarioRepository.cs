using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Dtos.Usuario;
using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Models.Entities.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ClubCanotajeAPI.Repositories.Usuario
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _db;
        public UsuarioRepository(AppDbContext db) => _db = db;

        public async Task<UsuarioSistema?> GetByUsernameAsync(string username) =>
            await _db.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Remador)
                .Include(u => u.Instructor)
                .FirstOrDefaultAsync(u => u.Username == username && u.Activo);

        public async Task RegistrarAccesoAsync(int id)
        {
            var u = await _db.Usuarios.FindAsync(id);
            if (u is not null)
            {
                u.UltimoAcceso = DateTime.Now;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteUsernameAsync(string username) =>
            await _db.Usuarios.AnyAsync(u => u.Username == username);

        public async Task<bool> ExisteRutAsync(string rut) =>
            await _db.Remadores.AnyAsync(r => r.Rut == rut);

        public async Task<bool> ExisteEmailRemadorAsync(string email) =>
            await _db.Remadores.AnyAsync(r => r.Email == email);

        public async Task<RolSistema?> GetRolByNombreAsync(string nombre) =>
            await _db.RolesSistema.FirstOrDefaultAsync(r => r.Nombre == nombre);

        public async Task<RolSistema?> GetRolByIdAsync(int idRol) =>
            await _db.RolesSistema.FindAsync(idRol);

        public async Task<List<RolSistema>> GetRolesAsync() =>
            await _db.RolesSistema.OrderBy(r => r.Id).ToListAsync();

        public async Task<int> GetCategoriaDefaultAsync()
        {
            var cat = await _db.CategoriasRemador
                .FirstAsync(c => c.Nombre == "Adulto");
            return cat.Id;
        }

        public async Task<int> GetEstadoRemadorActivoAsync()
        {
            var estado = await _db.EstadosRemador
                .FirstAsync(e => e.Nombre == "Activo");
            return estado.Id;
        }

        public async Task<UsuarioSistema> CrearConRemadorAsync(Remador remador, UsuarioSistema usuario)
        {
            var strategy = _db.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _db.Database.BeginTransactionAsync();
                try
                {
                    await _db.Remadores.AddAsync(remador);
                    await _db.SaveChangesAsync();

                    usuario.IdRemador = remador.Id;

                    await _db.Usuarios.AddAsync(usuario);
                    await _db.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return usuario;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<UsuarioSistema> CrearAsync(UsuarioSistema usuario)
        {
            await _db.Usuarios.AddAsync(usuario);
            await _db.SaveChangesAsync();
            return usuario;
        }

        public async Task<UsuarioSistema?> GetByEmailRemadorAsync(string email) =>
        await _db.Usuarios
            .Include(u => u.Remador)
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Remador != null && u.Remador.Email == email);

        public async Task<Remador?> GetRemadorByEmailAsync(string email) =>
            await _db.Remadores.FirstOrDefaultAsync(r => r.Email == email);

        public async Task ActivarUsuarioAsync(UsuarioSistema usuario)
        {
            usuario.Activo = true;
            usuario.EmailVerificado = true;
            usuario.FechaVerificacion = DateTime.Now;
            await _db.SaveChangesAsync();
        }

        public async Task CambiarPasswordAsync(UsuarioSistema usuario, string nuevoPasswordHash)
        {
            usuario.PasswordHash = nuevoPasswordHash;
            await _db.SaveChangesAsync();
        }


        public async Task<UsuarioInfoDto?> GetInfoCompletaAsync(int id)
        {
            var usuario = await _db.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Remador)
                    .ThenInclude(r => r!.Categoria)
                .Include(u => u.Remador)
                    .ThenInclude(r => r!.Estado)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null) return null;

            return new UsuarioInfoDto
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Rol = usuario.Rol.Nombre,
                Activo = usuario.Activo,
                UltimoAcceso = usuario.UltimoAcceso,
                FechaCreacion = usuario.FechaCreacion,
                EmailVerificado = usuario.EmailVerificado,

                Remador = usuario.Remador is null ? null : new RemadorInfoDto
                {
                    Id = usuario.Remador.Id,
                    NombreCompleto = usuario.Remador.NombreCompleto,
                    Rut = usuario.Remador.Rut,
                    Email = usuario.Remador.Email,
                    Telefono = usuario.Remador.Telefono,
                    FechaNacimiento = usuario.Remador.FechaNacimiento,
                    Genero = usuario.Remador.Genero,
                    Categoria = usuario.Remador.Categoria.Nombre,
                    Estado = usuario.Remador.Estado.Nombre,
                    FechaIngreso = usuario.Remador.FechaIngreso,
                    FotoUrl = usuario.Remador.FotoUrl,
                    CertMedicaVence = usuario.Remador.CertMedicaVence,
                    TieneRemoPropio = usuario.Remador.TieneRemoPropio,
                    TieneSalvavidasPropio = usuario.Remador.TieneSalvavidasPropio,
                    Observaciones = usuario.Remador.Observaciones,
                    NombreContactoEmergencia = usuario.Remador.NombreContactoEmergencia,
                    TelefonoEmergencia = usuario.Remador.TelefonoEmergencia,
                }
            };
        }
    }
}
