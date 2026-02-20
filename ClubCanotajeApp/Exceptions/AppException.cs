namespace ClubCanotajeAPI.Exceptions
{
    /// <summary>
    /// Excepción base para todas las excepciones de la aplicación
    /// </summary>
    public abstract class AppException : Exception
    {
        public string ErrorCode { get; }
        public string ErrorType { get; }
        public int? StatusCode { get; }
        protected AppException(string message, string errorCode, string errorType, int? statusCode = null)
            : base(message)
        {
            ErrorCode = errorCode;
            ErrorType = errorType;
            StatusCode = statusCode;
        }
    }
    /// <summary>
    /// Excepción para errores de validación de datos de entrada (request)
    /// StatusCode por defecto: 400 Bad Request
    /// </summary>
    public class DataSourceException : AppException
    {
        public DataSourceException(string message, string code, int? statusCode = null)
            : base(message, code, "DS", statusCode)
        {
        }
    }
    /// <summary>
    /// Excepción para errores de base de datos o modelo de datos
    /// StatusCode por defecto: 500 Internal Server Error
    /// </summary>
    public class ModelException : AppException
    {
        public ModelException(string message, string code, int? statusCode = null)
            : base(message, code, "SM", statusCode)
        {
        }
    }
    /// <summary>
    /// Excepción para errores en llamadas a servicios externos
    /// StatusCode por defecto: 502 Bad Gateway
    /// </summary>
    public class ClientException : AppException
    {
        public ClientException(string message, string code, int? statusCode = null)
            : base(message, code, "CE", statusCode)
        {
        }
    }
    /// <summary>
    /// Excepción para errores internos del servidor no controlados
    /// StatusCode por defecto: 500 Internal Server Error
    /// </summary>
    public class ServerException : AppException
    {
        public ServerException(string message, string code, int? statusCode = null)
            : base(message, code, "SE", statusCode)
        {
        }
    }
}
