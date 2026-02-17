using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;
using ClubCanotajeAPI.Models.Entities;

namespace ClubCanotajeAPI.Services
{
    public class EmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration config)
        {
            _apiKey = config["SendGrid:ApiKey"]!;
            _fromEmail = config["SendGrid:FromEmail"]!;
            _fromName = config["SendGrid:FromName"] ?? "Club Canotaje";
        }

        public async Task<bool> EnviarCodigoVerificacionAsync(
            string destinatario,
            string codigo,
            string tipo)
        {
            var asunto = tipo == TipoVerificacion.Registro
                ? "Verifica tu cuenta"
                : "Recuperación de contraseña";

            var html = tipo == TipoVerificacion.Registro
                ? GenerarHtmlRegistro(codigo)
                : GenerarHtmlResetPassword(codigo);

            return await EnviarEmailAsync(destinatario, asunto, html);
        }

        public async Task<bool> EnviarRecordatorioSalidaAsync(
            string destinatario,
            string nombreRemador,
            DateTime fechaHora,
            string canoa,
            string tipo,
            int idSalida)
        {
            var html = GenerarHtmlRecordatorio(nombreRemador, fechaHora, canoa, tipo, idSalida);
            return await EnviarEmailAsync(
                destinatario,
                $"Recordatorio: Salida {fechaHora:dd/MM/yyyy}",
                html);
        }

        private async Task<bool> EnviarEmailAsync(string to, string subject, string html)
        {
            try
            {
                var client = new SendGridClient(_apiKey);
                var msg = new SendGridMessage
                {
                    From = new EmailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    HtmlContent = html
                };
                msg.AddTo(new EmailAddress(to));

                var response = await client.SendEmailAsync(msg);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Log.Error($"Error enviando email a {to}: {ex.Message}");
                return false;
            }
        }

        private string GenerarHtmlRegistro(string codigo) => $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background: #007bff; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                    .content {{ background: #f9f9f9; padding: 30px; border: 1px solid #ddd; border-top: none; border-radius: 0 0 5px 5px; }}
                    .code {{ font-size: 32px; font-weight: bold; color: #007bff; letter-spacing: 5px; text-align: center; padding: 20px; background: white; border-radius: 5px; margin: 20px 0; }}
                    .footer {{ text-align: center; padding: 20px; color: #666; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>¡Bienvenido al Club de Canotaje! 🛶</h1>
                    </div>
                    <div class='content'>
                        <p>Gracias por registrarte. Para activar tu cuenta, ingresa el siguiente código:</p>
                        <div class='code'>{codigo}</div>
                        <p><strong>Este código expira en 15 minutos.</strong></p>
                        <p>Si no solicitaste este registro, puedes ignorar este email.</p>
                    </div>
                    <div class='footer'>
                        <p>Club de Canotaje Polinésico - Santiago, Chile</p>
                    </div>
                </div>
            </body>
            </html>";

        private string GenerarHtmlResetPassword(string codigo) => $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background: #dc3545; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                    .content {{ background: #f9f9f9; padding: 30px; border: 1px solid #ddd; border-top: none; border-radius: 0 0 5px 5px; }}
                    .code {{ font-size: 32px; font-weight: bold; color: #dc3545; letter-spacing: 5px; text-align: center; padding: 20px; background: white; border-radius: 5px; margin: 20px 0; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Recuperación de Contraseña 🔐</h1>
                    </div>
                    <div class='content'>
                        <p>Recibimos una solicitud para restablecer tu contraseña. Usa este código:</p>
                        <div class='code'>{codigo}</div>
                        <p><strong>Este código expira en 15 minutos.</strong></p>
                        <p>Si no solicitaste este cambio, ignora este email y tu contraseña permanecerá sin cambios.</p>
                    </div>
                </div>
            </body>
            </html>";

        private string GenerarHtmlRecordatorio(
            string nombre, DateTime fecha, string canoa, string tipo, int idSalida) => $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; color: #333; }}
                    .btn {{ background: #007bff; color: white; padding: 12px 30px; text-decoration: none; 
                            border-radius: 5px; display: inline-block; margin: 15px 0; }}
                </style>
            </head>
            <body>
                <h2>¡Hola {nombre}! 👋</h2>
                <p>Te recordamos tu salida programada:</p>
                <ul>
                    <li><strong>Fecha:</strong> {fecha:dddd dd/MM/yyyy}</li>
                    <li><strong>Hora:</strong> {fecha:HH:mm}</li>
                    <li><strong>Canoa:</strong> {canoa}</li>
                    <li><strong>Tipo:</strong> {tipo}</li>
                </ul>
                <a href='https://app.clubcanotaje.cl/salidas/{idSalida}/confirmar' class='btn'>
                    Confirmar Asistencia
                </a>
                <p>¡Nos vemos en el agua! 🌊</p>
            </body>
            </html>";
        }
}
