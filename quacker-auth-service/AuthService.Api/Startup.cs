using AuthService.Api.Data;
using AuthService.Api.Models;
using AuthService.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace AuthService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(
                    Configuration.GetConnectionString("AuthServiceDb"),
                    new MySqlServerVersion(new Version(8, 0, 23)),
                mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 2);
                });
            });

            services.AddScoped<IServiceBus, ServiceBus>();

            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Password complexity
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            });

                        // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]))
                };
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IServiceBus serviceBus)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePages();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ApplicationDbInitializer.SeedRoles(roleManager);
            ApplicationDbInitializer.SeedRootAdmin(userManager, serviceBus);
        }
    }
}
