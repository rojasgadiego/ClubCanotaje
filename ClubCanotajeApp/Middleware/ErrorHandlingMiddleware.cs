using ClubCanotajeAPI.Exceptions;
using ClubCanotajeAPI.Helper.Interface;
using ClubCanotajeAPI.Models.Dtos;

namespace ClubCanotajeAPI.Middleware
{
    /// <summary>
    /// Middleware que captura todas las excepciones y las convierte en respuestas HTTP formateadas
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IErrorResponseBuilder _errorBuilder;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IErrorResponseBuilder errorBuilder)
        {
            _next = next;
            _logger = logger;
            _errorBuilder = errorBuilder;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DataSourceException ex)
            {
                _logger.LogWarning($"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] Warning {nameof(ExceptionHandlingMiddleware)}: DataSourceException DS{ex.ErrorCode} - {ex.Message}");
                await HandleExceptionAsync(context, _errorBuilder.BuildDataSourceError(ex.ErrorCode, ex.Message, ex?.StatusCode));
            }
            catch (ModelException ex)
            {
                _logger.LogWarning($"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] Warning {nameof(ExceptionHandlingMiddleware)}: ModelException SM{ex.ErrorCode} - {ex.Message}");
                await HandleExceptionAsync(context, _errorBuilder.BuildModelError(ex.ErrorCode, ex.Message, ex?.StatusCode));
            }
            catch (ClientException ex)
            {
                _logger.LogWarning($"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] Warning {nameof(ExceptionHandlingMiddleware)}: ClientException CE{ex.ErrorCode} - {ex.Message}");
                await HandleExceptionAsync(context, _errorBuilder.BuildClientError(ex.ErrorCode, ex.Message, ex?.StatusCode));
            }
            catch (ServerException ex)
            {
                _logger.LogError($"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] Error {nameof(ExceptionHandlingMiddleware)}: ServerException SE{ex.ErrorCode} - {ex.Message}{(ex.InnerException != null ? " | " + ex.InnerException.Message : "")}");
                await HandleExceptionAsync(context, _errorBuilder.BuildServerError(ex.ErrorCode, ex.Message, ex?.StatusCode));
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] Error {nameof(ExceptionHandlingMiddleware)}: Unhandled exception - {ex.Message}{(ex.InnerException != null ? " | " + ex.InnerException.Message : "")}");
                await HandleExceptionAsync(context, _errorBuilder.BuildServerError("9999", "Error inesperado en el servidor"));
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, ErrorResponseApi errorResponse)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorResponse.Error.StatusCode;
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}