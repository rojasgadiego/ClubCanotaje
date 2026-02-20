using ClubCanotajeAPI.Helper.Interface;
using ClubCanotajeAPI.Models.Dtos;

namespace ClubCanotajeAPI.Helper
{
    public class ErrorResponseBuilder : IErrorResponseBuilder
    {
        private readonly IConfiguration _configuration;

        public ErrorResponseBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ErrorResponseApi BuildError(string errorCode, string code, string message, int? statusCode = null)
        {
            return new ErrorResponseApi
            {
                Error = new DetailResponse
                {
                    StatusCode = statusCode ?? 500,
                    Name = errorCode,
                    Code = $"{errorCode}{code}",
                    Message = message
                }
            };
        }

        public ErrorResponseApi BuildClientError(string code, string message, int? statusCode = null)
        {
            try
            {
                string errorCode = _configuration["ErrorCode:ClientError:Name"];
                string statusCodeStr = statusCode.ToString() ?? _configuration["ErrorCode:ClientError:StatusCode"];

                // Validar nulls
                if (string.IsNullOrEmpty(errorCode)) errorCode = "CE";
                if (string.IsNullOrEmpty(statusCodeStr))
                {
                    return BuildError(errorCode, code, message, 400);
                }

                int statusCodee = int.Parse(statusCodeStr);
                return BuildError(errorCode, code, message, statusCode);
            }
            catch
            {
                return BuildError("CE", code, message, 400);
            }
        }

        public ErrorResponseApi BuildDataSourceError(string code, string message, int? statusCode = null)
        {
            try
            {
                string errorCode = _configuration["ErrorCode:DataSourceError:Name"];
                string statusCodeStr = statusCode.ToString() ?? _configuration["ErrorCode:DataSourceError:StatusCode"];

                // Validar nulls
                if (string.IsNullOrEmpty(errorCode)) errorCode = "DS";
                if (string.IsNullOrEmpty(statusCodeStr))
                {
                    return BuildError(errorCode, code, message, 400);
                }

                int statusCodee = int.Parse(statusCodeStr);
                return BuildError(errorCode, code, message, statusCode);
            }
            catch
            {
                return BuildError("DS", code, message, 400);
            }
        }

        public ErrorResponseApi BuildModelError(string code, string message, int? statusCode = null)
        {
            try
            {
                string errorCode = _configuration["ErrorCode:ModelError:Name"];
                string statusCodeStr = statusCode.ToString() ?? _configuration["ErrorCode:ModelError:StatusCode"];

                // Validar nulls
                if (string.IsNullOrEmpty(errorCode)) errorCode = "SM";
                if (string.IsNullOrEmpty(statusCodeStr))
                {
                    return BuildError(errorCode, code, message, 400);
                }

                int statusCodee = int.Parse(statusCodeStr);
                return BuildError(errorCode, code, message, statusCode);
            }
            catch
            {
                return BuildError("SM", code, message, 400);
            }
        }

        public ErrorResponseApi BuildServerError(string code, string message, int? statusCode = null)
        {
            try
            {
                string errorCode = _configuration["ErrorCode:ServerError:Name"];
                string statusCodeStr = statusCode.ToString() ?? _configuration["ErrorCode:ServerError:StatusCode"];

                // Validar nulls
                if (string.IsNullOrEmpty(errorCode)) errorCode = "SE";
                if (string.IsNullOrEmpty(statusCodeStr))
                {
                    return BuildError(errorCode, code, message, 500);
                }

                int statusCodee = int.Parse(statusCodeStr);
                return BuildError(errorCode, code, message, statusCode);
            }
            catch
            {
                return BuildError("SE", code, message, 500);
            }
        }
    }
}
