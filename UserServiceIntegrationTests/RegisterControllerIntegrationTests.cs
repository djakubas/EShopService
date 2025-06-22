using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UserService.IntegrationTests;
public class RegisterControllerIntegrationTests : IClassFixture<WebApplicationFactory<UserService.Program>>
{

    private readonly HttpClient _httpClient;
    private WebApplicationFactory<UserService.Program> _applicationFactory;

    public RegisterControllerIntegrationTests(WebApplicationFactory<UserService.Program> applicationFactory)
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
    public async Task RegisterService_ShouldCreateUser_ReturnSuccess_ShouldLoginNewUser_ReturnToken()
    {
        var body = new LoginRequest
        {
            Username = "newUser",
            Password = "test1" //password meets requirements 
        };
        var bodyJson = JsonContent.Create(body);
        var result = await _httpClient.PostAsync("/Register", bodyJson);
        result.EnsureSuccessStatusCode();

        var resultLogin = await _httpClient.PostAsync("/Login", bodyJson);
        resultLogin.EnsureSuccessStatusCode();
        var token = await resultLogin.Content.ReadFromJsonAsync<JwtTokenResponse>();
        Assert.NotNull(token.Token);
    }
}
