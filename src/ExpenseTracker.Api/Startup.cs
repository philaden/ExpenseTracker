using System;
using ExpenseTracker.Api.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env {get;}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            ServiceConfiguration.AddDbConfig(services, Configuration);
            ServiceConfiguration.RegisterDependencies(services);
            ServiceConfiguration.AddDocumentationServices(services, Configuration);

            services.AddHttpContextAccessor();
            services.AddMvcCore().AddControllersAsServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            if (!Env.EnvironmentName.Equals("production", StringComparison.CurrentCultureIgnoreCase))
            {
                app.UseDeveloperExceptionPage();
            };

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", $"Expense Tracker API V1");
            });

            app.UseForwardedHeaders();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            ApplicationLogging.ConfigureLogger(loggerFactory);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
