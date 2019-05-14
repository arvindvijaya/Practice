using Extensions.ApiBase.Common;
using Extensions.ApiBase.Filters;
using Extensions.ApiBase.Interfaces;
using Extensions.ApiBase.Services;
using Extensions.Logging;
using Extensions.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Extensions.ApiBase
{
    /// <summary>
    /// encapsulate common functionality in startupbase
    /// </summary>
    public abstract class StartupBase
    {
        private string OAUTH_POLICY_NAME = "oAuthToken";

        public static readonly DateTime ServiceStartTime;

        protected bool ConvertEnumToString = true;

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        static StartupBase()
        {
            ServiceStartTime = DateTime.Now;
        }

        public StartupBase(IConfiguration configuration,IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// configure services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddClientApiLoggingAudit(Configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelAttribute));
                //add authentication filter
                options.Filters.Add(new AuthorizeFilter(OAUTH_POLICY_NAME));
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = JsonSerializer.Settings.ContractResolver;
                if (ConvertEnumToString)
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });

            services.AddoAuthTokenSecurity(OAUTH_POLICY_NAME, Configuration);

            //add httpcontext accessor provided by aspnet
            services.AddSingleton<ICurrentUserAccessor,HttpContextCurrentUserAccessor>();
            //add default implementation for callcontext accessor
            services.AddSingleton<ICallContextAccessor, DefaultCallContextAccessor>();
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCors(o => o.AddPolicy("EnableCors", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSwaggerGen(swagger =>
            {
                swagger.DescribeAllEnumsAsStrings();
                swagger.DescribeAllParametersInCamelCase();
                swagger.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "My First Swagger" });
            });

            RegisterService(services);

            services.AddSingleton(services.BuildServiceProvider());
        }

        public void Configure(IApplicationBuilder app,
            IServiceProvider serviceProvider)
        {
            //add logging to the pipeline
            app.UseClientApiLogging(Configuration, serviceProvider);


            app.UseCors("EnableCors");

            //add swagger to the pipeline
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My First Swagger");
            });

            app.UseMvc();
        }

        public virtual IServiceCollection RegisterService(IServiceCollection services)
        {
            return services;
        }

        private string GetServiceDescription()
        {
            return string.Format("<b>Environment{0}</b>",HostingEnvironment.EnvironmentName);
        }
    }
}
