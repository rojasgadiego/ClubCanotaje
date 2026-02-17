using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace ClubCanotajeAPI.Repositories.Verificacion
{
    public class VerificacionRepository
    {
        private readonly AppDbContext _db;
        public VerificacionRepository(AppDbContext db) => _db = db;

        public async Task<CodigoVerificacion> CrearCodigoAsync(
            string email, string tipo, int minutosExpiracion = 15)
        {
            // Invalidar códigos anteriores del mismo tipo
            var anteriores = await _db.CodigosVerificacion
                .Where(c => c.Email == email && c.Tipo == tipo && !c.Usado)
                .ToListAsync();

            foreach (var c in anteriores)
                c.Usado = true;

            // Generar nuevo código de 6 dígitos
            var codigo = new Random().Next(100000, 999999).ToString();

            var nuevoCodigo = new CodigoVerificacion
            {
                Email = email,
                Codigo = codigo,
                Tipo = tipo,
                Expira = DateTime.Now.AddMinutes(minutosExpiracion),
                FechaCreacion = DateTime.Now
            };

            await _db.CodigosVerificacion.AddAsync(nuevoCodigo);
            await _db.SaveChangesAsync();

            return nuevoCodigo;
        }

        public async Task<CodigoVerificacion?> ValidarCodigoAsync(
            string email, string codigo, string tipo)
        {
            var verif = await _db.CodigosVerificacion
                .FirstOrDefaultAsync(c =>
                    c.Email == email &&
                    c.Codigo == codigo &&
                    c.Tipo == tipo &&
                    !c.Usado);

            if (verif is null) return null;

            // Incrementar intentos fallidos
            verif.Intentos++;
            await _db.SaveChangesAsync();

            // Validar si es vigente
            return verif.EstaVigente ? verif : null;
        }

        public async Task MarcarComoUsadoAsync(CodigoVerificacion codigo)
        {
            codigo.Usado = true;
            await _db.SaveChangesAsync();
        }

        public async Task LimpiarCodigosExpiradosAsync()
        {
            var expirados = await _db.CodigosVerificacion
                .Where(c => c.Expira < DateTime.Now.AddDays(-1))
                .ToListAsync();

            _db.CodigosVerificacion.RemoveRange(expirados);
            await _db.SaveChangesAsync();
        }
    }
}
