using Extensions.CoreBase;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Swagger.Filters
{
    public class oAuthTokenHeaderOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ControllerAttributes()
                .Union(context.ApiDescription.ActionAttributes())
                .OfType<AllowAnonymousAttribute>().Any())
                return;

            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter()
            {
                Name = HeaderConstants.IvUserNameInHeader,
                In = "header",
                Description = "oAuth Token",
                Required = true,
                Type = "string"
            });
        }
    }
}
