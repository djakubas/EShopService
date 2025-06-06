using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
namespace Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                var result = JsonSerializer.Serialize(new { error = ex.Message, test = "created in middleware" });
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                await context.Response.WriteAsync(result);
            }
        }
    }
}
