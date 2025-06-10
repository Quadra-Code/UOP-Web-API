using System.Security.Claims;

namespace UOP.Web.API.Middlewares
{
    public class AuthUserMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //public async Task InvokeAsync(HttpContext context, IUserService userService)
        //{
        //    var auth0UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (!string.IsNullOrEmpty(auth0UserId))
        //    {
        //        var localUser = await userService.GetUserAsync(auth0UserId, default);
        //        if (localUser != null)
        //        {
        //            context.Items["LocalUser"] = localUser;
        //        }
        //    }

        //    await _next(context);
        //}
    }
}
