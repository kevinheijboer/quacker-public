using FluentAssertions;
using AuthService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Net;
using AuthService.Api.Controllers;
using System.Net.Http.Json;
using AuthService.Api.Payloads.Requests;

namespace AuthService.Tests.IntegrationTests
{
    public class AuthIntegrationTests : IntegrationTest
    {
        public AuthIntegrationTests()
            : base(Guid.NewGuid().ToString())
        {
        }

        [Fact]
        public async Task RegisterAccount_WithExistingUsername_Should_ReturnBadRequest()
        {
            var registrationRequest = new UserRegistrationRequest()
            {
                Username = "test",
                Email = "test2@example.com",
                Birthdate = DateTime.Now,
                Password = "testtest123"
            };

            var response = await TestClient.PostAsJsonAsync("/auth/register", registrationRequest);
            
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var response2 = await TestClient.PostAsJsonAsync("/auth/register", registrationRequest);

            response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_WithWrongPassword_Should_ReturnUnauthorized()
        {
            var registrationRequest = new UserRegistrationRequest()
            {
                Username = "test",
                Email = "test2@example.com",
                Birthdate = DateTime.Now,
                Password = "testtest123"
            };

            await TestClient.PostAsJsonAsync("/auth/register", registrationRequest);

            var loginRequest = new UserLoginRequest()
            {
                Username = "wrong",
                Password = "wrong"
            };

            var response = await TestClient.PostAsJsonAsync("/auth/signin", loginRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_WithCorrectCredentials_And_Username_Should_ReturnOk()
        {
            var registrationRequest = new UserRegistrationRequest()
            {
                Username = "test",
                Email = "test2@example.com",
                Birthdate = DateTime.Now,
                Password = "testtest123"
            };

            await TestClient.PostAsJsonAsync("/auth/register", registrationRequest);

            var loginRequest = new UserLoginRequest()
            {
                Username = "test",
                Password = "testtest123"
            };

            var response = await TestClient.PostAsJsonAsync("/auth/signin", loginRequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Login_WithCorrectCredentials_And_Email_Should_ReturnOk()
        {
            var registrationRequest = new UserRegistrationRequest()
            {
                Username = "test",
                Email = "test2@example.com",
                Birthdate = DateTime.Now,
                Password = "testtest123"
            };

            await TestClient.PostAsJsonAsync("/auth/register", registrationRequest);

            var loginRequest = new UserLoginRequest()
            {
                Username = "test2@example.com",
                Password = "testtest123"
            };

            var response = await TestClient.PostAsJsonAsync("/auth/signin", loginRequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
