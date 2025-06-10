namespace UOP.Web.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class PermissionAuthorizeAttribute : Attribute
    {
        public string RequiredPermission { get; set; }
    }
}
