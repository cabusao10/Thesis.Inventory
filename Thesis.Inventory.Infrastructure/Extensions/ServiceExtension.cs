using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Thesis.Inventory.Infrastructure.Repository;
using Thesis.Inventory.Infrastructure.UnitOfWork;

namespace Thesis.Inventory.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.AddDbContext<ThesisDbContext>((provider, options) =>
            {
                options.UseSqlServer(
                    configuration["DefaultConnection"],
                    b => b.MigrationsAssembly(typeof(ThesisDbContext).Assembly.FullName));
                options.UseLazyLoadingProxies(true);

            });

            services.AddScoped<IThesisDBContext, ThesisDbContext>();

            services.AddTransient(typeof(IApplicationRepository<>), typeof(ApplicationRepository<>));
            services.AddTransient<IThesisUnitOfWork, ThesisUnitOfWork>();
        }
    }
}
