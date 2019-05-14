using Swashbuckle.AspNetCore.Swagger;

namespace Extensions.Swagger.Models
{
    public class SwaggerInfo :Info
    {
        public bool UseTestValueHeader { get; set; }
        internal static SwaggerInfo CurrentValue { get; set; }
    }
}
