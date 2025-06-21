using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using User.Domain;
using User.Domain.Models;
using UserService;
using System.IdentityModel.Tokens.Jwt;
using System;
using User.Application.DTO;
using System.Net.Http.Headers;
namespace UserService.IntegrationTests
{
    public class LoginControllerIntegrationTests : IClassFixture<WebApplicationFactory<UserService.Program>>
    {

        private readonly HttpClient _httpClient;
        private WebApplicationFactory<UserService.Program> _applicationFactory;

        public LoginControllerIntegrationTests(WebApplicationFactory<UserService.Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
            //_applicationFactory = applicationFactory.WithWebHostBuilder(builder =>
            //{
            //    builder.ConfigureServices(services =>
            //    {
            //        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DbContext>));

            //        if (dbContextOptions != null)
            //            services.Remove(dbContextOptions);

            //        services.AddDbContext<DbContext>(options => options.UseInMemoryDatabase("TestDB"));
            //    });
            //});

            //seed DB here if needed
            
            _httpClient = _applicationFactory.CreateClient();
        }

        [Fact]
        public async Task LoginService_LoginAdminOk_ShouldReturnJWTToken_ShouldAllowAllAccess()
        {
            //login
            var body = new LoginRequest
            {
                Username = "Admin",
                Password = "test" //correct password
            };
            var bodyJson = JsonContent.Create(body);
            var result = await _httpClient.PostAsync("/Login", bodyJson);
            result.EnsureSuccessStatusCode();
            var token = await result.Content.ReadFromJsonAsync<JwtTokenResponse>();
            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            string[] endpoints = { "/Admin", "/Employee", "/Client" };

            
            foreach (var endpoint in endpoints)
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
                
                var pageResult = await _httpClient.SendAsync(request);
                
                pageResult.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task LoginClient_ShouldReturnToken_UpdateUser_ShouldReturnUpdatedUser()
        {
            //login
            var body = new LoginRequest
            {
                Username = "TestUser",
                Password = "test" //correct password
            };
            var bodyJson = JsonContent.Create(body);
            var result = await _httpClient.PostAsync("/Login", bodyJson);
            result.EnsureSuccessStatusCode();
            var token = await result.Content.ReadFromJsonAsync<JwtTokenResponse>();
            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            //Update Logged current User
            var dto = new UserUpdateDto () { Email = "test@email.com", UserName = "TestUserUpdated" };
            var updateBodyJson = JsonContent.Create(dto);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            var resultUpdate = await _httpClient.PatchAsync("/Me", updateBodyJson);
            resultUpdate.EnsureSuccessStatusCode();
            var user = await resultUpdate.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(user);
            Assert.Equal("test@email.com", user.Email);
            Assert.Equal("TestUserUpdated", user.UserName);

        }
    }
}