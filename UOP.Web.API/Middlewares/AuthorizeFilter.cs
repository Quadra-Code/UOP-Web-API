using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using UOP.Domain.Interfaces;
using UOP.Web.API.Attributes;
using UOP.Web.API.Extensions;

namespace UOP.Web.API.Middlewares
{
    public class AuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IPermissionService _permissionService;

        public AuthorizeFilter(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async System.Threading.Tasks.Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var authorizeAttributes = controllerActionDescriptor.MethodInfo
                    .GetCustomAttributes(typeof(PermissionAuthorizeAttribute), true) as PermissionAuthorizeAttribute[];

                if (authorizeAttributes != null && authorizeAttributes.Any())
                {
                    foreach (var attribute in authorizeAttributes)
                    {
                        var requiredPermission = attribute.RequiredPermission;
                        var userId = context.HttpContext.GetLocalUserId();

                        if (!userId.HasValue)
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }

                        var hasPermission = await _permissionService.HasPermissionAsync(userId.Value, requiredPermission);
                        if (!hasPermission)
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                }
            }
        }
    }
}
