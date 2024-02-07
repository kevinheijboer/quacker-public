using FluentAssertions;
using AccountService.Api.Models;
using AccountService.Api.Models.Payloads.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;

namespace AccountService.Tests.IntegrationTests
{
    public class AccountsIntegrationTests : IntegrationTest
    {
        public AccountsIntegrationTests()
            : base(Guid.NewGuid().ToString())
        {
        }

        [Fact]
        public async Task GetAccount_ReturnsAccount()
        {
            var response = await TestClient.GetAsync($"/accounts/{SeedData.User3.Username}");
            var account = await response.Content.ReadAsAsync<Account>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            account.UserId.Should().Be(SeedData.User3.UserId);
        }

        [Fact]
        public async Task UpdateAccount_Should_UpdateAccountInfo()
        {
            var editProfileRequest = new EditProfileRequest()
            {
                Name = "newname"
            };

            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(JsonSerializer.Serialize(editProfileRequest)), "profile");
                var response = await TestClient.PutAsync($"/accounts/{SeedData.User3.UserId}", formData);
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            var response2 = await TestClient.GetAsync($"/accounts/{SeedData.User3.Username}");
            var account = await response2.Content.ReadAsAsync<Account>();

            response2.StatusCode.Should().Be(HttpStatusCode.OK);
            account.Name.Should().Be(editProfileRequest.Name);
        }
    }
}
