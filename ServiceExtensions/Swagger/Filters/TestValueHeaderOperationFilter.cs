using Extensions.CoreBase;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Extensions.Swagger.Filters
{
    public class TestValueHeaderOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        { 
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter()
            {
                Name = HeaderConstants.TestValueHeaderName,
                In = "header",
                Description = "Generic all-purpose header value for local test",
                Required = true,
                Type = "string"
            });
        }
    }
}
