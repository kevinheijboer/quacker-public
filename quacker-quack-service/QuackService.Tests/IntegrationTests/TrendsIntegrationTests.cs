using FluentAssertions;
using QuackService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace QuackService.Tests.IntegrationTests
{
    public class TrendsIntegrationTests : IntegrationTest
    {
        public TrendsIntegrationTests()
       : base(Guid.NewGuid().ToString())
        {
        }

        [Fact]
        public async Task GetTrends_ReturnsOnlyThisWeek()
        {
            var response = await TestClient.GetAsync("/trends");

            IEnumerable<string> topics = await response.Content.ReadAsAsync<IEnumerable<string>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            topics.Should().HaveCount(1);
        }

    }
}
