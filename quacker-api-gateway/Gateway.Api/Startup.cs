using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Kubernetes;

namespace Gateway.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot().AddKubernetes().AddCacheManager(settings => settings.WithDictionaryHandle());

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        if (CurrentEnvironment.IsDevelopment())
                        {
                            builder.WithOrigins("https://quacker.nl",
                                                "https://www.quacker.nl",
                                                "http://localhost:8080")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                        }
                        else if (CurrentEnvironment.IsStaging())
                        {
                            builder.WithOrigins("https://staging.quacker.nl")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                        }
                        else
                        {
                            builder.WithOrigins("https://quacker.nl",
                                                "https://www.quacker.nl")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                        }
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    if (CurrentEnvironment.IsDevelopment())
                    {
                        await context.Response.WriteAsync("API Gateway (v0.1) (dev)");
                    }
                    else if (CurrentEnvironment.IsStaging())
                    {
                        await context.Response.WriteAsync("API Gateway (v0.1) (staging)");
                    }
                    else if (CurrentEnvironment.IsProduction())
                    {
                        await context.Response.WriteAsync("API Gateway (v0.1)");
                    }
                });
            });

            app.UseOcelot().Wait();
        }
    }
}
