using System.Security.Claims;
using Quiz.Application.Abstractions;
using Quiz.Core.Common;
using Serilog.Context;

namespace Quiz.Api.Middlewares;

public class UserBlockMiddleware(IUserCache cache) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            var isBlocked = await cache.UserIsBlockedAsync(userId);
            if (isBlocked)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(DomainErrors.User.UserBlocked);
                return;
            }
        }
        
        await next(context);
    }
}