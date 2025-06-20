using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EShop.Domain.Repositories;
using EShop.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EShopService.IntegrationTests;

public class CreditCardControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public CreditCardControllerIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));

                    services.Remove(dbContextOptions);

                    services
                        .AddDbContext<DataContext>(options => options.UseInMemoryDatabase("MyDBForTest"));
                });
            });
        _client = _factory.CreateClient();
    }


    // GetValidation Tests

    [Theory]
    [InlineData("4532289052809181", true, HttpStatusCode.OK)] // Valid Visa
    [InlineData("234564", false, HttpStatusCode.BadRequest)] // Too short (exception)
    [InlineData("2345643456543456765434567654345676543", false, HttpStatusCode.BadRequest)] // Too long (exception)
    [InlineData("4532289052809181a", false, HttpStatusCode.BadRequest)] // Non-numeric (exception)
    public async Task GetValidation_WhenLuhnIsPassed_ReturnsOkStatusCode(string cardNumber, bool expectedResult, HttpStatusCode expectedStatus)
    {
        // Act
        var response = await _client.GetAsync($"/api/CreditCard/Validate/{cardNumber}");

        // Assert
        Assert.Equal(expectedStatus, response.StatusCode);
    }





    // GetType Tests

    [Theory]
    [InlineData("4532289052809181", "Visa", HttpStatusCode.OK)] // Valid Visa
    [InlineData("5530016454538418", "Mastercard", HttpStatusCode.OK)] // Valid Mastercard
    [InlineData("378523393817437", "American_Express", HttpStatusCode.OK)] // Valid American Express
    [InlineData("00000000000000", "Unknown", HttpStatusCode.OK)] // Invalid (too short)
    [InlineData("6011123456789012", "Discover", HttpStatusCode.OK)] // Discover (not in enum, returns string)
    public async Task GetType_WhenGivenValidCardNumber_ReturnsOkStatusCode(string cardNumber, string expectedType, HttpStatusCode expectedStatus)
    {
        // Act
        var response = await _client.GetAsync($"/api/CreditCard/CardType/{cardNumber}");

        // Assert
        Assert.Equal(expectedStatus, response.StatusCode);
    }
}