using ClubCanotajeAPI.Models.Entities;

namespace ClubCanotajeAPI.Repositories.Verificacion;

public interface IVerificacionRepository
{
    Task<CodigoVerificacion> CrearCodigoAsync(string email, string tipo, int minutosExpiracion = 15);
    Task<CodigoVerificacion?> ValidarCodigoAsync(string email, string codigo, string tipo);
    Task MarcarComoUsadoAsync(CodigoVerificacion codigo);
    Task LimpiarCodigosExpiradosAsync();
}