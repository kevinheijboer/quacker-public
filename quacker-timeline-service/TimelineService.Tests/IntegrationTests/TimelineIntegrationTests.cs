using FluentAssertions;
using TimelineService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TimelineService.Tests.IntegrationTests
{
    public class TimelineIntegrationTests : IntegrationTest
    {
        public TimelineIntegrationTests()
            : base(Guid.NewGuid().ToString())
        {
        }

        [Fact]
        public async Task GetTimeline_ReturnsFollowingTweets()
        {
            var response = await TestClient.GetAsync($"/timeline");
            var timeline = await response.Content.ReadAsAsync<IEnumerable<Quack>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            timeline.Should().HaveCount(1);
        }
    }
}
