using ClubCanotajeAPI.Models.Dtos.Transbank.Request;
using ClubCanotajeAPI.Models.Dtos.Transbank.Response;
using Transbank.Common;
using Transbank.Webpay.Common;
using Transbank.Webpay.WebpayPlus;

namespace ClubCanotajeAPI.Services
{
    public class TransBankService
    {

        private Transaction tx;
        private TransBankResponseGlobal response;

        public TransBankService()
        {
            tx = new Transaction(new Options(IntegrationCommerceCodes.WEBPAY_PLUS, IntegrationApiKeys.WEBPAY, WebpayIntegrationType.Test));
            response = new TransBankResponseGlobal();
        }


        public TransBankResponseGlobal CrearTransaccion(CreateTransaction request)
        {
            if (request == null)
                return CrearError("El request no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(request.session_id))
                return CrearError("session_id es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.buy_order))
                return CrearError("buy_order es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.return_url))
                return CrearError("return_url es obligatorio.");

            if (request.amount <= 0)
                return CrearError("El monto debe ser mayor a cero.");

            try
            {
                var response = tx.Create(
                    request.buy_order,
                    request.session_id,
                    request.amount,
                    request.return_url
                );

                if (response == null)
                    return CrearError("No se recibió respuesta de Transbank.");

                var responseTransbank = new
                {
                    Url = response.Url,
                    Token = response.Token,
                    UrlCompleta = $"{response.Url}?token_ws={response.Token}"
                };

                return new TransBankResponseGlobal
                {
                    Exito = true,
                    Message = "Transacción generada correctamente.",
                    Data = responseTransbank
                };
            }
            catch (Exception ex)
            {
                // Idealmente loggear aquí
                return CrearError("Error al crear la transacción.", ex.Message);
            }
        }


        public TransBankResponseGlobal confirmrTransaccion (ConfirmTransaction request)
        {
            if (string.IsNullOrWhiteSpace(request.Token))
                return CrearError("el token es obligatorio.");

            try
            {
                var response = tx.Commit(request.Token);
                var responseTransBank = new ConfirmTransactionResponse
                {
                    vci = response.Vci,
                    amount = response.Amount,
                    status = response.Status,
                    buy_order = response.BuyOrder,
                    session_id = response.SessionId,
                    response_code = response.ResponseCode,

                };

                return new TransBankResponseGlobal
                {
                    Exito = true,
                    Message = "Transacción generada correctamente.",
                    Data = responseTransBank
                };
            }
            catch (Exception ex)
            {
                // Idealmente loggear aquí
                return CrearError("Error al confirmar la transacción.", ex.Message);
            }


        } 


        private TransBankResponseGlobal CrearError(string mensaje, string detalle = null)
        {
            return new TransBankResponseGlobal
            {
                Exito = false,
                Message = detalle == null ? mensaje : $"{mensaje} Detalle: {detalle}",
                Data = null
            };
        }




    }
}
