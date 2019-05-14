using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Extensions.Swagger.Filters
{
    public class OperationIdFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var descriptor = (ControllerActionDescriptor)context.ApiDescription.ActionDescriptor;
            operation.OperationId = $"{descriptor.ControllerName.Replace("Controller", string.Empty)}_{descriptor.ActionName}";
        }
    }
}
