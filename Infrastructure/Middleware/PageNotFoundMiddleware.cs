using System.Text.Json;
using Microsoft.AspNetCore.Http;
namespace Infrastructure.Middleware 
{
    public class PageNotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public PageNotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 404)
            {
                var result = JsonSerializer.Serialize(new {message = "What you are looking for is not here"});
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            
            

        }
    }
}
