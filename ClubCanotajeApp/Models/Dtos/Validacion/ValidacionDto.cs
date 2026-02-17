namespace ClubCanotajeAPI.Models.Dtos.Validacion
{
    public record VerificarEmailRequest(string Email, string Codigo);
    public record ReenviarCodigoRequest(string Email);
    public record RecuperarPasswordRequest(string Email);
    public record ResetPasswordRequest(string Email, string Codigo, string NuevaPassword);

}
