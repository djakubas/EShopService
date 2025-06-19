using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middleware
{
    public class ServiceUnderMaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _maintenance;
        public ServiceUnderMaintenanceMiddleware(RequestDelegate next, bool maintenance)
        {
            _next = next;
            _maintenance = maintenance;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_maintenance)
            {
                context.Response.StatusCode = 503;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = "Service under maintenance" });

                await context.Response.WriteAsync(result);
            }
            else
            {
                await _next(context);
            }
        }
    }
}
