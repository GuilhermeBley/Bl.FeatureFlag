using Bl.FeatureFlag.Domain.Primitive;
using Bl.FeatureFlag.Domain.Primitive.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Bl.FeatureFlag.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CoreException e)
        {
            _logger.LogError(e, e.Message);

            context.Response.StatusCode = (int)ExtractStatusCode(e.StatusCode); 
            context.Response.ContentType = "application/json";
            
            var errorsRange = e is AggregateCoreException
                ? ((AggregateCoreException)e).InnerExceptions
                : [e];

            var errorResponse = new
            {
                Errors = errorsRange.Select(x => new
                {
                    Code = x.StatusCode,
                    Description = x.StatusCode.ToString(),
                    Message = x.Message
                })
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }

    private static HttpStatusCode ExtractStatusCode(CoreExceptionCode code)
    {
        var numberStr = ((int)code).ToString();

        var statusCode = int.TryParse(
            string.Concat(numberStr.Take(3)),
            out var firstThreeDigit);

        if (firstThreeDigit >= 200 && firstThreeDigit < 500 &&
            Enum.IsDefined(typeof(HttpStatusCode), firstThreeDigit))
        {
            return (HttpStatusCode)firstThreeDigit;
        }
        else
        {
            return HttpStatusCode.BadRequest;
        }

    }
}
