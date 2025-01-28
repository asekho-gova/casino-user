using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
using Casino.User.Api;
using Casino.User.Api.Models;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Casino.User.Api.Services;
using Casino.User.Api.Persistence;
using System.IO;

namespace Casino.User.Api.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private string _dbPath;
        private const string TestDbName = "casinoUsers.test.db";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _dbPath = Path.Combine(Directory.GetCurrentDirectory(), TestDbName);
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
            File.Copy("../../../src/Casino.User.Api/casinoUsers.db", _dbPath);

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IConnectionProvider));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }
                        services.AddSingleton<IConnectionProvider>(new ConnectionProvider(_dbPath));
                    });
                });

            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory?.Dispose();
            _client?.Dispose();
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
            File.Copy("../../../src/Casino.User.Api/casinoUsers.db", _dbPath);
        }

        [Test]
        public async Task GetUser_WhenUserExists_ReturnsOkWithUser()
        {
            var response = await _client.GetAsync("/api/user/1");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var user = await response.Content.ReadFromJsonAsync<CasinoUser>();
            Assert.NotNull(user);
            Assert.That(user!.UserId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUser_WhenUserDoesNotExist_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/user/999");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task CreateUser_WithValidData_ReturnsCreatedWithUser()
        {
            var newUser = new CreateUserRequest
            {
                Username = "testuser1",
                Password = "Welcome123!",
                Email = "test@example.com",
                MobilePhoneNumber = "1234567890"
            };

            var response = await _client.PostAsJsonAsync("/api/user", newUser);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            
            var createdUser = await response.Content.ReadFromJsonAsync<CasinoUser>();
            Assert.NotNull(createdUser);
            Assert.That(createdUser!.Username, Is.EqualTo(newUser.Username));
            Assert.That(createdUser.Email, Is.EqualTo(newUser.Email));
            Assert.That(createdUser.MobilePhoneNumber, Is.EqualTo(newUser.MobilePhoneNumber));
        }

        [Test]
        public async Task CreateUser_WithInvalidData_ReturnsBadRequest()
        {
            var invalidUser = new CreateUserRequest
            {
                Username = "",
                Password = "",
                Email = "invalid-email"
            };

            var response = await _client.PostAsJsonAsync("/api/user", invalidUser);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UpdateBalance_WithValidAmount_ReturnsOkWithUpdatedBalance()
        {
            var response = await _client.PostAsync("/api/user/1/updateBalance?amount=100", null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var result = await response.Content.ReadFromJsonAsync<UpdateBalanceResponse>();
            Assert.NotNull(result);
            Assert.That(result!.UserId, Is.EqualTo(1));
            Assert.That(result.UpdateAmount, Is.EqualTo(100));
            
            var userResponse = await _client.GetAsync("/api/user/1");
            var user = await userResponse.Content.ReadFromJsonAsync<CasinoUser>();
            Assert.NotNull(user);
            Assert.That(user!.Balance, Is.EqualTo(result.Balance));
        }

        [Test]
        public async Task UpdateBalance_WithInvalidUserId_ReturnsBadRequest()
        {
            var response = await _client.PostAsync("/api/user/999/updateBalance?amount=100", null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task DeleteUser_WhenUserExists_ReturnsNoContent()
        {
            var response = await _client.DeleteAsync("/api/user/2");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            
            var getResponse = await _client.GetAsync("/api/user/2");
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task DeleteUser_WhenUserDoesNotExist_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/user/999");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
