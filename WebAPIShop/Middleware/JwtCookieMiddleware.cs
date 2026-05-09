namespace WebAPIShop.Middleware;

public class JwtCookieMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization") &&
            context.Request.Cookies.TryGetValue("jwt", out string? token))
        {
            context.Request.Headers.Append("Authorization", $"Bearer {token}");
        }
        await next(context);
    }
}

public static class JwtCookieMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtCookieMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<JwtCookieMiddleware>();
}
