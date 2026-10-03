using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LightCap.InvestmentApi.Application.Common.Exceptions;

public class ApiException : Exception
{
    private readonly string _title;
    private readonly Error[] _errors;
    private readonly int _statusCode;

    public ApiException(
        string title,
        string message,
        Error[] errors,
        HttpStatusCode statusCode,
        Exception? innerException = null)
        : base(message, innerException) // ✅ IMPORTANT
    {
        _title = title;
        _errors = errors;
        _statusCode = (int)statusCode;
    }

    public string Title => _title;

    public Dictionary<string, object> Extensions
        => new() { ["errors"] = _errors };

    public int StatusCode => _statusCode;

    // ✅ Updated static methods

    public static ApiException NotFound(
        Error error,
        string message = "The requested resource was not found.",
        Exception? inner = null)
        => new("Not Found", message, [error], HttpStatusCode.NotFound, inner);

    public static ApiException BadRequest(
        Error error,
        string message = "Invalid request parameters.",
        Exception? inner = null)
        => new("Bad Request", message, [error], HttpStatusCode.BadRequest, inner);

    public static ApiException BadRequest(
        Error[] errors,
        string message = "Invalid request parameters.",
        Exception? inner = null)
        => new("Bad Request", message, errors, HttpStatusCode.BadRequest, inner);

    public static ApiException Unauthorized(
        Error error,
        string message = "Authentication required.",
        Exception? inner = null)
        => new("Unauthorized", message, [error], HttpStatusCode.Unauthorized, inner);

    public static ApiException Forbidden(
        Error error,
        string message = "Insufficient permissions.",
        Exception? inner = null)
        => new("Forbidden", message, [error], HttpStatusCode.Forbidden, inner);

    public static ApiException InternalServerError(
        Error[] errors,
        string message = "An unexpected error occurred.",
        Exception? inner = null)
        => new("Internal Server Error", message, errors, HttpStatusCode.InternalServerError, inner);
}