using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace WebAPIShop.Extensions
{
    public static class RateLimitSetup
    {
        public static void AddCustomRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    var user = httpContext.User.Identity?.Name ?? "guest";
                    var partitionKey = $"{user}_{ip}";

                    return RateLimitPartition.GetSlidingWindowLimiter(partitionKey, _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 30,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 3,
                        QueueLimit = 0,
                        AutoReplenishment = true
                    });
                });
            });
        }
    }
}