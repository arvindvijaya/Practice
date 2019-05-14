using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extensions.ApiBase.Interfaces;
using Extensions.ApiBase.Services;
using Extensions.Security.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SampleApplication;

namespace WebApi
{
    public class Startup : Extensions.ApiBase.StartupBase
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration) : base(configuration, env)
        { 
        }

        public override IServiceCollection RegisterService(IServiceCollection services)
        {
            services.AddSeiServices(Configuration.GetConnectionString("MsSql:PNotes"));

            services.AddSingleton<ICallContextAccessor,DefaultCallContextAccessor>(); 

            return services;
        }  
    }
}
