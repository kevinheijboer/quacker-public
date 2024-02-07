using QuackService.Api.Data;
using QuackService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuackService.Tests.IntegrationTests
{
    public class SeedData
    {
        public static Quack QuackOne { get; private set; }
        public static Quack QuackTwo { get; private set; }
        public static Topic TopicOne { get; private set; }
        public static Topic LastWeekTopic { get; private set; }

        static SeedData()
        {
            QuackOne = new Quack
            {
                Id = Guid.NewGuid(),
                Username = "testman",
                UserId = Guid.NewGuid(),
                Message = "This is a quack",
                CreatedOn = DateTime.Now
            };
            QuackTwo = new Quack
            {
                Id = Guid.NewGuid(),
                Username = "testusername",
                UserId = Guid.NewGuid(),
                Message = "This is a quack",
                CreatedOn = DateTime.Now
            };

            TopicOne = new Topic
            {
                Id = Guid.NewGuid(),
                Value = "#trend",
                CreatedOn = DateTime.Now.AddMinutes(-5),
            };

            LastWeekTopic = new Topic
            {
                Id = Guid.NewGuid(),
                Value = "#oudetrend",
                CreatedOn = DateTime.Now.AddDays(-8),
            };

        }

        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.AddRange(QuackOne, QuackTwo, TopicOne, LastWeekTopic);
            db.SaveChanges();
        }
    }
}
