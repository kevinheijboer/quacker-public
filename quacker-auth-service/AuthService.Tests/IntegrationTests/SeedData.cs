using AuthService.Api.Data;
using AuthService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Tests.IntegrationTests
{
    public class SeedData
    {
        public static ApplicationUser User1 { get; private set; }
        public static ApplicationUser User2 { get; private set; }
        public static ApplicationUser User3 { get; private set; }

        static SeedData()
        {
            User1 = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testname",
                Email = "testemail"
            };
        }

        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.AddRange(User1);
            db.SaveChanges();
        }
    }
}
