using FluentAssertions;
using FollowService.Api.Models;
using FollowService.Api.Models.Payloads.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FollowService.Tests.IntegrationTests
{
    public class FollowIntegrationTests : IntegrationTest
    {
        public FollowIntegrationTests()
            : base(Guid.NewGuid().ToString())
        {
        }

        [Fact]
        public async Task GetFollowers_ReturnsFollowers()
        {
            var response = await TestClient.GetAsync($"/followers/{SeedData.User3.Username}");
            var followers = await response.Content.ReadAsAsync<IEnumerable<User>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            followers.Should().HaveCount(2);
        }


        [Fact]
        public async Task GetFollowers_ReturnsFollowing()
        {
            var response = await TestClient.GetAsync($"/following/{SeedData.User3.Username}");
            var followers = await response.Content.ReadAsAsync<IEnumerable<User>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            followers.Should().HaveCount(1);
        }

        [Fact]
        public async Task Follow_AlreadyFollowing_ReturnsBadRequest()
        {
            var request = new FollowRequest()
            {
                UserId = SeedData.User1.UserId,
            };

            var response = await TestClient.PostAsJsonAsync("/follow", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Follow_ShouldAddToFollowers()
        {
            var response1 = await TestClient.GetAsync($"/followers/{SeedData.User2.Username}");
            var followers = await response1.Content.ReadAsAsync<IEnumerable<User>>();
            followers.Should().HaveCount(0);

            var request = new FollowRequest()
            {
                UserId = SeedData.User2.UserId,
            };

            var response2 = await TestClient.PostAsJsonAsync("/follow", request);
            response2.StatusCode.Should().Be(HttpStatusCode.OK);

            var response3 = await TestClient.GetAsync($"/followers/{SeedData.User2.Username}");
            var followers2 = await response3.Content.ReadAsAsync<IEnumerable<User>>();
            followers2.Should().HaveCount(1);
        }


        [Fact]
        public async Task CheckIfFollows_Should_ReturnTrue_When_Following()
        {
            var response = await TestClient.GetAsync($"/Follow/IsFollowing/{SeedData.User1.Username}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var follows = await response.Content.ReadAsAsync<bool>();

            follows.Should().BeTrue();
        }
    }
}
