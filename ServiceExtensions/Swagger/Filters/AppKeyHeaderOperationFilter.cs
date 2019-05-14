using Extensions.CoreBase;
using Swagger.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Swagger.Filters
{
    public class AppKeyHeaderOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if(context.ApiDescription
                .ControllerAttributes()
                .Union(context.ApiDescription.ActionAttributes())
                .OfType<AppKeyNotRequiredAttribute>().Any())
                return;

            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter()
            {
                Name = HeaderConstants.AppKeyHeaderName,
                In = "header",
                Description = "Application Key",
                Required = true,
                Type = "string"
            });

        }
    }
}
