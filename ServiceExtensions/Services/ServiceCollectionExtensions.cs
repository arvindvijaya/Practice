using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SampleApplication.DbContext;
using SampleApplication.Interfaces;
using SampleApplication.Services;
using System;

namespace SampleApplication
{
    /// <summary>
    /// extensions to add dbcontext to the project
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSeiServices(this IServiceCollection services,string connectionString)
        {
            var options = new DbContextOptionsBuilder<PNoteServicesDbContext>().UseSqlServer(connectionString).Options;

            services.AddSingleton(options);
            services.AddTransient<IPNoteServicesDbContext, PNoteServicesDbContext>();
            services.AddSingleton<Func<IPNoteServicesDbContext>>(prov => () => prov.GetRequiredService<IPNoteServicesDbContext>());
            services.AddSingleton<IPNotesService, PNotesServiceImpl>();

            return services;
        }
    }
}
