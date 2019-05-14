using Extensions.Swagger.Filters;
using Extensions.Swagger.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace Extensions.Swagger.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHondaSwagger (this IServiceCollection services,
            IOptions<SwaggerInfo> information,bool includexmlDocs = true)
        {
            SwaggerInfo.CurrentValue = information.Value;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", information.Value);

                c.OperationFilter<OperationIdFilter>();
                c.OperationFilter<DefaultResponseFilter>();

                if(information.Value.UseTestValueHeader)
                    c.OperationFilter<TestValueHeaderOperationFilter>();

                if(includexmlDocs)
                {
                    var basePath = System.AppContext.BaseDirectory;
                    foreach(var lib in DependencyContext.Default.CompileLibraries)
                    {
                        var xmlPath = Path.Combine(basePath, $"{lib.Name}.xml");
                        if(File.Exists(xmlPath))
                        {
                            c.IncludeXmlComments(xmlPath);
                        }
                    }
                }

                c.MapType<object>(() => new Schema { Type = "object" });
                c.IgnoreObsoleteProperties();
                c.DescribeAllEnumsAsStrings();
            });

            return services;
        }
    }
}
