using ClubCanotajeAPI.Models.Dtos.Common;
using System.Net;
using System.Text.Json;

namespace ClubCanotajeAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try { await _next(ctx); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado en {Path}", ctx.Request.Path);
                await HandleAsync(ctx, ex);
            }
        }

        private static async Task HandleAsync(HttpContext ctx, Exception ex)
        {
            var (status, msg) = ex switch
            {
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "No autorizado."),
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "Error interno del servidor.")
            };

            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)status;

            var response = ApiResponse.Fail(msg);
            var json = JsonSerializer.Serialize(response,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            await ctx.Response.WriteAsync(json);
        }
    }
}
