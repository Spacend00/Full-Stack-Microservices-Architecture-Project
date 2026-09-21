using Microsoft.Extensions.Caching.Distributed;
using System.Net;

namespace UserService.API.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenBlacklistMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, IDistributedCache cache)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if(!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var accessToken = authHeader.Substring("Bearer ".Length).Trim();
                var blacklistCacheKey = $"blacklistedToken:{accessToken}";

                var isBlacklisted = await cache.GetStringAsync(blacklistCacheKey);

                if (!string.IsNullOrEmpty(isBlacklisted))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"message\": \"Token iptal edilmiştir. Lütfen tekrar giriş yapın.\"}");
                    return;
                }
            }

            await _next(context);
        }
    }
}
