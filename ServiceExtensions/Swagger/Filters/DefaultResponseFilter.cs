using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Swagger.Filters
{
    public class DefaultResponseFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Responses == null)
                operation.Responses = new Dictionary<string, Response>();

            if(!operation.Responses.ContainsKey("400"))
            {
                operation.Responses.Add("400", new Response
                {Description = "Bad request.See return for details.",
                Schema  = new Schema {Ref = "#/definitions/ValidationErrorResponse" }
                });
            }

            if (!operation.Responses.ContainsKey("401") &&
                !context.ApiDescription.ControllerAttributes().Union(context.ApiDescription.ActionAttributes())
                .OfType<AllowAnonymousAttribute>().Any())
            {
                operation.Responses.Add("401", new Response
                {
                    Description = "Authorization failed."
                });

                operation.Responses.Add("403", new Response
                {
                    Description = "unauthorized access to this data."
                });
            }

            if (!operation.Responses.ContainsKey("500"))
            {
                operation.Responses.Add("500", new Response
                {
                    Description = "Server error.See return for details.",
                    Schema = new Schema { Ref = "#/definitions/ErrorResponse"}
                }); 
            } 
        }
    }
}
