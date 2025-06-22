using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.IntegrationTests;
public class MeControllerIntegrationTests : IClassFixture<WebApplicationFactory<UserService.Program>>
{

    private readonly HttpClient _httpClient;
    private WebApplicationFactory<UserService.Program> _applicationFactory;

    public MeControllerIntegrationTests(WebApplicationFactory<UserService.Program> applicationFactory)
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

}
