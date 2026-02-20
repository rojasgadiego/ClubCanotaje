namespace ClubCanotajeAPI.Models.Dtos
{
    /// <summary>
    /// Respuesta estándar de error para todas las excepciones de la aplicación.
    /// </summary>
    public class ErrorResponse
    {
        public ErrorDetail Error { get; set; } = null!;
    }

    /// <summary>
    /// Detalle del error retornado al cliente.
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>HTTP status code. Ej: 400, 500, 502</summary>
        public int StatusCode { get; set; }

        /// <summary>Tipo de error. Ej: "DS", "SM", "CE", "SE"</summary>
        public string Type { get; set; } = null!;

        /// <summary>Código único del error. Ej: "DS001", "CE002"</summary>
        public string Code { get; set; } = null!;

        /// <summary>Mensaje legible para el cliente.</summary>
        public string Message { get; set; } = null!;

        /// <summary>Stack trace. Solo visible en ambiente Development.</summary>
        public string? Detail { get; set; }
    }
}
