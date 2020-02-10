using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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

        public static void AddDocumentationServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "V1 Docs", Version = "v1" });
            });

        }
    }
}
