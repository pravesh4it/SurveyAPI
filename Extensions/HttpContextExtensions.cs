using Microsoft.AspNetCore.Http;
using System.Linq;
namespace ABC.Extensions
{
    public static class HttpContextExtensions
    {
        
            public static string GetClientIp(this HttpContext context)
            {
                if (context == null)
                    return null;

                // Cloudflare
                if (context.Request.Headers.TryGetValue("CF-Connecting-IP", out var cfIp))
                    return cfIp.FirstOrDefault();

                // Reverse proxy
                if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
                    return forwarded.FirstOrDefault()?.Split(',').First();

                return context.Connection.RemoteIpAddress?.ToString();
            }
        }
    
}
