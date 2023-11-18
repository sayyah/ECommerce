using ECommerce.Domain.Entities.Helper;
using System.Net;

namespace ECommerce.Domain.Exceptions;

public class AppException(ResultCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception,
        object additionalData)
    : Exception(message, exception)
{
    public AppException()
        : this(ResultCode.Error)
    {
    }

    public AppException(ResultCode statusCode)
        : this(statusCode, null)
    {
    }

    public AppException(string message)
        : this(ResultCode.Error, message)
    {
    }

    public AppException(ResultCode statusCode, string message)
        : this(statusCode, message, HttpStatusCode.InternalServerError)
    {
    }

    public AppException(string message, object additionalData)
        : this(ResultCode.Error, message, additionalData)
    {
    }

    public AppException(ResultCode statusCode, object additionalData)
        : this(statusCode, null, additionalData)
    {
    }

    public AppException(ResultCode statusCode, string message, object additionalData)
        : this(statusCode, message, HttpStatusCode.InternalServerError, additionalData)
    {
    }

    public AppException(ResultCode statusCode, string message, HttpStatusCode httpStatusCode)
        : this(statusCode, message, httpStatusCode, null)
    {
    }

    public AppException(ResultCode statusCode, string message, HttpStatusCode httpStatusCode, object additionalData)
        : this(statusCode, message, httpStatusCode, null, additionalData)
    {
    }

    public AppException(string message, Exception exception)
        : this(ResultCode.Error, message, exception)
    {
    }

    public AppException(string message, Exception exception, object additionalData)
        : this(ResultCode.Error, message, exception, additionalData)
    {
    }

    public AppException(ResultCode statusCode, string message, Exception exception)
        : this(statusCode, message, HttpStatusCode.InternalServerError, exception)
    {
    }

    public AppException(ResultCode statusCode, string message, Exception exception, object additionalData)
        : this(statusCode, message, HttpStatusCode.InternalServerError, exception, additionalData)
    {
    }

    public AppException(ResultCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception)
        : this(statusCode, message, httpStatusCode, exception, null)
    {
    }

    public HttpStatusCode HttpStatusCode { get; set; } = httpStatusCode;
    public ResultCode ApiStatusCode { get; set; } = statusCode;
    public object AdditionalData { get; set; } = additionalData;
}
