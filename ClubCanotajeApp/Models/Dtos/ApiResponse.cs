namespace ClubCanotajeAPI.Models.Dtos
{
    /// <summary>
    /// Respuesta estándar de éxito para todos los endpoints de la aplicación.
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>Indica si la operación fue exitosa.</summary>
        public bool Success { get; set; }

        /// <summary>Mensaje descriptivo de la operación.</summary>
        public string Message { get; set; } = null!;

        /// <summary>Datos retornados al cliente.</summary>
        public T? Data { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "OK") =>
            new() { Success = true, Message = message, Data = data };
    }
}
