using PersonalFinanceTracker.Repositories;

namespace PersonalFinanceTracker.Middlwares;

public class AutorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AutorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var pattern = "^/User/(SignIN|Register)$";

        if (httpContext.Request.Path.StartsWithSegments("/User") && httpContext.Request.Path != "/User/SignIN" && httpContext.Request.Path != "/User/Register")
        {
            var token = httpContext.Session.GetString("AuthToken");
            var _userRepository = httpContext.RequestServices.GetRequiredService<IUserRepository>();
            if (string.IsNullOrEmpty(token) && !_userRepository.CheckTokenExpire(token))
            {
                httpContext.Response.Redirect("/User/SignIn");
                return;
            }
        }
        await _next(httpContext);
    }
}

public static class AuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AutorizationMiddleware>();
    }
}