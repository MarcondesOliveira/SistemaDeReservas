using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace SistemaDeReservas.Infrastructure
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                context.Response.StatusCode,
                Message = "Internal Server Error.",
                // Simplificar a resposta de erro para evitar problemas de serialização complexa
                Error = exception.Message // Apenas a mensagem de erro é retornada
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                // Remover o ReferenceHandler para evitar ciclos de objeto
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
