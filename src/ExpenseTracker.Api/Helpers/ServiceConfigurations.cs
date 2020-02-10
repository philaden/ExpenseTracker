using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.Repositories;
using ExpenseTracker.Api.Services.Interfaces;
using ExpenseTracker.Api.Services.Implementations;

namespace ExpenseTracker.Api.Helpers
{
    public static class ServiceConfiguration
    {
        public static void AddDbConfig(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ExpenseTrackerContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ExpenseMonitorDb")));
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IExpenseService, ExpenseService>();

        }
    }
}
