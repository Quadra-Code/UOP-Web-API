using UOP.Application.Common.DTOs;

namespace UOP.Web.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static UserDto? GetLocalUser(this HttpContext context)
        {
            return context.Items["LocalUser"] as UserDto;
        }

        public static Guid? GetLocalEmployeeId(this HttpContext context)
        {
            var user = context.GetLocalUser();
            return user?.EmployeeId;
        }

        public static Guid? GetLocalUserId(this HttpContext context)
        {
            var user = context.GetLocalUser();
            return user?.UserId;
        }

        public static Guid GetLocalTenantId(this HttpContext context)
        {
            if (context.Items.TryGetValue("TenantID", out var value) && value is Guid tenantId)
            {
                return tenantId;
            }

            throw new InvalidOperationException("TenantID is not available in the current HttpContext.");
        }

    }
}
