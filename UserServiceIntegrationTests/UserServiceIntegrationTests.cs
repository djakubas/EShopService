using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
//using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using User.Domain;
using User.Domain.Models;

namespace UserServiceIntegrationTests
{
    public class UserServiceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly HttpClient _httpClient;
        private WebApplicationFactory<Program> _applicationFactory;

        public UserServiceIntegrationTests(WebApplicationFactory<Program> applicationFactory)
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
            
            _httpClient = _applicationFactory.CreateClient();

            
        }

        [Fact]
        public async Task LoginService_LoginAdminOk_ShouldReturnJWTToken_ShouldAllowAdminAccess()
        {
            var body = new LoginRequest
            {
                Username = "Admin",
                Password = "test"
            };

            var token = await _httpClient.PostAsJsonAsync("/Login", body);
            Assert.NotNull(token); 
        }
         

        //public void LoginService_LoginClientOk_ShouldreturnJWTToken_ShouldForbidAdminAccess()
        //{

        //}
    }
}