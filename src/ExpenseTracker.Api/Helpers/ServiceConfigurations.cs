using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Api.Repositories;
using ExpenseTracker.Api.Services.Interfaces;
using ExpenseTracker.Api.Services.Implementations;
using Microsoft.OpenApi.Models;
using ExpenseTracker.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Helpers
{
    public static class ServiceConfiguration
    {
        public static void AddDbConfig(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ExpenseTrackerContext>(options => options.UseNpgsql(Configuration.GetConnectionString("MyDbConnection")));
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IExpenseService, ExpenseService>();

        }

        public static IServiceCollection  AddDocumentationServices(this IServiceCollection services, IConfiguration configuration)
        {
            string ApplicationName = "Expense Tracker";

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.FullName);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{ApplicationName} API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

            });
            return services;

        }
    }
}
