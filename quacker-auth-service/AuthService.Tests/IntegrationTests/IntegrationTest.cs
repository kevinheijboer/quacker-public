using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AuthService.Api;
using AuthService.Api.Data;
using AuthService.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Tests.IntegrationTests
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
                        {
                            options.UseInMemoryDatabase(this.DatabaseName);
                        });

                        services.AddScoped<IServiceBus, MockServiceBus>();

                        services.AddControllers(options =>
                        {
                            options.Filters.Add(new AllowAnonymousFilter());
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
