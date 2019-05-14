using Microsoft.AspNetCore.Builder;

namespace Extensions.Swagger.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHondaSwagger (this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            { 
                c.SwaggerEndpoint("/swagger/v1/swagger.json","Service API V1"); 
            });

            return app;
        }
    }
}
