using Extensions.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Extensions.ApiBase
{
    /// <summary>
    /// programbase to configure asp.netcore webhost
    /// </summary>
    public abstract class ProgramBase
    {
        public static IWebHostBuilder CreateBaseBuilder(params string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseIISIntegration()
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddSerilogLogging(hostingContext);
            });
        }
    }
}
