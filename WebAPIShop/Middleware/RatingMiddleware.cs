using Services;
using Entitys;
using PresidentsApp.Middlewares;

namespace WebAPIShop.Middleware
{
    public class RatingMiddleware
    {
        private readonly RequestDelegate _next;
       


        public RatingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IRatingService ratingService)
        {
            Rating newRating=new Rating();
            newRating.Host=httpContext.Request.Host.Value;
            newRating.Method=httpContext.Request.Method;
            newRating.Path=httpContext.Request.Path;
            newRating.Referer=httpContext.Request.Headers.Referer;
            newRating.UserAgent=httpContext.Request.Headers.UserAgent;
            newRating.RecordDate=DateTime.Now;
            
            await ratingService.AddRating(newRating);
            await _next(httpContext);
        }
    }

    public static class RatingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRatingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RatingMiddleware>();
        }
    }
}
