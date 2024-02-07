using TimelineService.Api.Data;
using TimelineService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineService.Tests.IntegrationTests
{
    public class SeedData
    {
        public static User User1 { get; private set; }
        public static User User2 { get; private set; }
        public static User User3 { get; private set; }
        public static Quack Quack1 { get; private set; }

        static SeedData()
        {
            User1 = new User()
            {
                UserId = Guid.NewGuid(),
                Username = "user1",
                Followers = new List<User>(),
                Following = new List<User>(),
            };
            User2 = new User()
            {
                UserId = Guid.NewGuid(),
                Username = "user2",
                Followers = new List<User>(),
                Following = new List<User>(),
            };
            User3 = new User()
            {
                UserId = Guid.NewGuid(),
                Username = "user3",
                Followers = new List<User>() { User1, User2 },
                Following = new List<User>() { User1 },
            };
            Quack1 = new Quack()
            {
                Id = Guid.NewGuid(),
                UserId = User1.UserId,
                Message = "Tweet from user1",
                Username = User1.Username,
                CreatedOn = DateTime.Now
            };
        }

        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.AddRange(User1, User2, User3, Quack1);
            db.SaveChanges();
        }
    }
}
