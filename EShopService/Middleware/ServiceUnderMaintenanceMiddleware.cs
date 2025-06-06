using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EShopService.Middleware
{
    public class ServiceUnderMaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _maintenance = false;
        public ServiceUnderMaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
            //_maintenance = maintenance;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_maintenance)
            {
                context.Response.StatusCode = 503;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = "Service under mantenance" });

                await context.Response.WriteAsync(result);
            }
            else
            {
                await _next(context);
            }
        }
    }
}
