using FluentAssertions;
using QuackService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Net;
using QuackService.Api.Controllers;
using QuackService.Api.Models.Payloads.Requests;
using System.Net.Http.Json;

namespace QuackService.Tests.IntegrationTests
{
    public class QuacksIntegrationTests : IntegrationTest
    {
        public QuacksIntegrationTests()
            : base(Guid.NewGuid().ToString())
        {
        }

        [Fact]
        public async Task GetQuacks_ReturnsTwo()
        {
            var response = await TestClient.GetAsync("/quacks");
            
            var quacks = await response.Content.ReadAsAsync<IEnumerable<Quack>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            quacks.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetQuacksFromUser_ReturnsOne()
        {
            var response = await TestClient.GetAsync("/quacks/user/testman");

            var quacks = await response.Content.ReadAsAsync<IEnumerable<Quack>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            quacks.Should().HaveCount(1);
        }

        [Fact]
        public async Task PostQuack_ReturnsQuack()
        {
            var getQuacksResponse = await TestClient.GetAsync("/quacks");

            var quacks = await getQuacksResponse.Content.ReadAsAsync<IEnumerable<Quack>>();

            // get quacks
            getQuacksResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            quacks.Should().HaveCount(2);

            var request = new QuackRequest()
            {
                Message = "This is a quack"
            };

            var response = await TestClient.PostAsJsonAsync("/quacks", request);
            var quack = await response.Content.ReadAsAsync<Quack>();

            // post quack
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            quack.Message.Should().Be(request.Message);

            var getQuacksResponse2 = await TestClient.GetAsync("/quacks");

            var quacks2 = await getQuacksResponse2.Content.ReadAsAsync<IEnumerable<Quack>>();

            // assert if quacks count incremented
            getQuacksResponse2.StatusCode.Should().Be(HttpStatusCode.OK);
            quacks2.Should().HaveCount(3);
        }
    }
}
