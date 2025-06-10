using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace UOP.Web.API.Attributes
{
    public class DefaultProducesResponseTypeConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            var method = action.ActionMethod;
            Type returnType = method.ReturnType;

            // Unwrap Task<T> to get the actual return type
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                returnType = returnType.GetGenericArguments()[0];
            }

            // Check if the return type is ActionResult<T>
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ActionResult<>))
            {
                Type successType = returnType.GetGenericArguments()[0];

                // Add response types based on HTTP method and conventions
                AddStandardResponseTypes(action, successType);
            }

            // Add default response for HTTP 500 (Internal Server Error)
            AddResponseTypeFilter(action, StatusCodes.Status500InternalServerError, typeof(string));
        }

        private void AddStandardResponseTypes(ActionModel action, Type successType)
        {
            // Determine the HTTP method
            var httpMethod = action.Attributes
                .OfType<HttpMethodAttribute>()
                .FirstOrDefault()
                ?.HttpMethods
                .FirstOrDefault();

            if (httpMethod == null)
            {
                return; // If no HTTP method is found, skip adding standard response types
            }

            switch (httpMethod.ToUpperInvariant())
            {
                case "GET":
                    AddResponseTypeFilter(action, StatusCodes.Status200OK, successType);
                    AddResponseTypeFilter(action, StatusCodes.Status404NotFound, typeof(string));
                    break;

                case "POST":
                    AddResponseTypeFilter(action, StatusCodes.Status201Created, successType);
                    AddResponseTypeFilter(action, StatusCodes.Status400BadRequest, typeof(string[]));
                    break;

                case "PUT":
                    AddResponseTypeFilter(action, StatusCodes.Status200OK, successType);
                    AddResponseTypeFilter(action, StatusCodes.Status400BadRequest, typeof(string[]));
                    AddResponseTypeFilter(action, StatusCodes.Status404NotFound, typeof(string));
                    break;

                case "DELETE":
                    AddResponseTypeFilter(action, StatusCodes.Status204NoContent, typeof(void));
                    AddResponseTypeFilter(action, StatusCodes.Status404NotFound, typeof(string));
                    break;

                default:
                    // For other HTTP methods, you can add additional conventions if needed
                    break;
            }
        }

        private void AddResponseTypeFilter(ActionModel action, int statusCode, Type type)
        {
            bool filterExists = action.Filters.Any(filter =>
                filter is ProducesResponseTypeAttribute responseAttr &&
                responseAttr.StatusCode == statusCode);

            if (!filterExists)
            {
                var attribute = new ProducesResponseTypeAttribute(type, statusCode);
                action.Filters.Add(attribute);
            }
        }
    }
}
