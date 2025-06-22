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
using Microsoft.AspNetCore.Mvc;
using EShopService.Controllers;
using EShop.Domain.Exceptions.CreditCard;
using EShopService;

namespace EShopService.IntegrationTests;

public class CreditCardControllerIntegrationTests : IClassFixture<WebApplicationFactory<EShopService.Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public CreditCardControllerIntegrationTests(WebApplicationFactory<EShopService.Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));

                    services.Remove(dbContextOptions!);

                    services
                        .AddDbContext<DataContext>(options => options.UseInMemoryDatabase("MyDBForTest"));
                });
            });
        _client = _factory.CreateClient();
    }


    // GetValidation Tests

    [Theory]
    [InlineData("4532289052809181", HttpStatusCode.OK)] // Valid Visa
    [InlineData("234564", HttpStatusCode.BadRequest)] // Too short (exception)
    [InlineData("2345643456543456765434567654345676543", HttpStatusCode.BadRequest)] // Too long (exception)
    [InlineData("4532289052809181a", HttpStatusCode.BadRequest)] // Non-numeric (exception)
    public async Task GetValidation_WhenLuhnIsPassed_ReturnsOk(string cardNumber, HttpStatusCode expectedStatus)
    {
        // Act
        var response = await _client.GetAsync($"/api/CreditCard/Validate/{cardNumber}");

        // Assert
        Assert.Equal(expectedStatus, response.StatusCode);
    }





    // GetType Tests

    [Theory]
    [InlineData("4532289052809181", HttpStatusCode.OK)] // Valid Visa
    [InlineData("5530016454538418", HttpStatusCode.OK)] // Valid Mastercard
    [InlineData("378523393817437", HttpStatusCode.OK)] // Valid American Express
    [InlineData("00000000000000",  HttpStatusCode.BadRequest)] // Unrecognized (not in enum)
    [InlineData("6011123456789012", HttpStatusCode.BadRequest)] // Discover (not in enum)
    public async Task GetType_WhenGivenValidCardNumber_ReturnsOkStatusCode(string cardNumber, HttpStatusCode expectedStatus)
    {
        // Act
        var response = await _client.GetAsync($"/api/CreditCard/CardType/{cardNumber}");
        // Assert
        Assert.Equal(expectedStatus, response.StatusCode);
    }
}