using AccountService.Api.Data;
using AccountService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Tests.IntegrationTests
{
    public class SeedData
    {
        public static Account User1 { get; private set; }
        public static Account User2 { get; private set; }
        public static Account User3 { get; private set; }

        static SeedData()
        {
            User1 = new Account()
            {
                UserId = Guid.NewGuid(),
                Username = "user1",
            };
            User2 = new Account()
            {
                UserId = Guid.NewGuid(),
                Username = "user2",
            };
            User3 = new Account()
            {
                UserId = Guid.NewGuid(),
                Username = "user3",
                Bio = "bio",
                Email = "user3@example.com",
                Location = "Eindhoven",
                Name = "user3",
                CreatedOn = DateTime.Now
            };
        }

        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.AddRange(User1, User2, User3);
            db.SaveChanges();
        }
    }
}
