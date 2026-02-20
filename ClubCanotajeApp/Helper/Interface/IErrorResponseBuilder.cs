using ClubCanotajeAPI.Models.Dtos;

namespace ClubCanotajeAPI.Helper.Interface
{
    public interface IErrorResponseBuilder
    {
        ErrorResponseApi BuildError(string errorCode, string code, string message, int? statusCode = null);
        ErrorResponseApi BuildClientError(string code, string message, int? statusCode = null);
        ErrorResponseApi BuildDataSourceError(string code, string message, int? statusCode = null);
        ErrorResponseApi BuildModelError(string code, string message, int? statusCode = null);
        ErrorResponseApi BuildServerError(string code, string message, int? statusCode = null);
    }
}
