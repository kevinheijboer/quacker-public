using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AccountService.Api;
using AccountService.Api.Data;
using AccountService.Api.Services;
using System.Linq;
using System.Net.Http;
using AccountService.Tests.IntegrationTests;
using System;
using AccountService.Api.Logic;

namespace AccountService.Tests.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        public string DatabaseName { get; set; }

        protected IntegrationTest(string databaseName)
        {

            this.DatabaseName = databaseName;
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<ApplicationDbContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseInMemoryDatabase(this.DatabaseName),
                            contextLifetime: ServiceLifetime.Transient,
                            optionsLifetime: ServiceLifetime.Singleton);

                        services.AddDbContextFactory<ApplicationDbContext>(options =>
                                options.UseInMemoryDatabase(this.DatabaseName),
                                ServiceLifetime.Singleton);

                        services.AddScoped<IS3Service, MockS3Service>();

                        services.AddControllers(options =>
                        {
                            options.Filters.Add(new AllowAnonymousFilter());
                        });

                        services
                            .AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                        services.AddAuthorization(options =>
                        {
                            options.AddPolicy("<Existing Policy Name>", builder =>
                            {
                                builder.AuthenticationSchemes.Add("Test");
                                builder.RequireAuthenticatedUser();
                            });
                            options.DefaultPolicy = options.GetPolicy("<Existing Policy Name>");
                        });

                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                            // Seed the database with some specific test data.
                            SeedData.InitializeDbForTests(db);
                        }
                    });
                });
            TestClient = appFactory.CreateClient();
        }
    }
}
