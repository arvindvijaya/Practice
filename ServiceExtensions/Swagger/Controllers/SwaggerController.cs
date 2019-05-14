using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Swagger.Filters;
using System.Reflection;

namespace Extensions.Swagger.Controllers
{
    [AllowAnonymous]
    [AppKeyNotRequired]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    [Route("swagger")]
    public class SwaggerController : Controller
    {
        private const string RESOURCE_NAME = "Extensions.Swagger.EmbeddedResources";

        [HttpGet("")]
        public IActionResult SwaggerDirectory()
        {
            return new VirtualFileResult("index.html", "text/html")
            {
                FileProvider = new EmbeddedFileProvider(GetType().GetTypeInfo().Assembly, RESOURCE_NAME)
            };
        }

        [HttpGet("index.html")]
        public IActionResult SwaggerInfoFile()
        {
            return RedirectToAction(nameof(SwaggerDirectory));
        }

        [HttpGet("skin.css")]
        public IActionResult SwaggerSkinFile()
        {
            return new VirtualFileResult("skin.css", "text/css")
            {
                FileProvider = new EmbeddedFileProvider(GetType().GetTypeInfo().Assembly, RESOURCE_NAME)
            };
        }

        [HttpGet("favicon.ico")]
        public IActionResult SwaggerFaviconFile()
        {
            return new VirtualFileResult("favicon.ico", "image/x-icon")
            {
                FileProvider = new EmbeddedFileProvider(GetType().GetTypeInfo().Assembly, RESOURCE_NAME)
            };
        }

        [HttpGet("logo_small.png")]
        public IActionResult SwaggerLogoSmallFile()
        {
            return new VirtualFileResult("logo_small.png", "image/png")
            {
                FileProvider = new EmbeddedFileProvider(GetType().GetTypeInfo().Assembly, RESOURCE_NAME)
            };
        }
    }
}
