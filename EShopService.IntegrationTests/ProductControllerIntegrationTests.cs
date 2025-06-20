using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using EShop.Domain.Models.Products;
using EShop.Domain.Models.Cart;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using System;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace EShopService.IntegrationTests;

public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
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





    // Get Tests

    [Fact]
    public async Task Get_ReturnsAllProducts_ExpectedTwoProducts()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Products.RemoveRange(dbContext.Products);
        dbContext.Products.AddRange(new Product { Name = "Product1" }, new Product { Name = "Product2" });
        await dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/product");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.Equal(2, products?.Count);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsProduct()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Products.RemoveRange(dbContext.Products);
        var product = new Product { Name = "Product1" };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        var id = product.Id;

        // Act
        var response = await _client.GetAsync($"/api/product/{id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Product>();
        Assert.Equal("Product1", result?.Name);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Products.RemoveRange(dbContext.Products);
        await dbContext.SaveChangesAsync();
        var nonExistingId = 999;

        // Act
        var response = await _client.GetAsync($"/api/product/{nonExistingId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }





    // Post Tests

    [Fact]
    public async Task Post_ValidProduct_ReturnsOk()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Products.RemoveRange(dbContext.Products);
        await dbContext.SaveChangesAsync();
        var product = new Product { Name = "Product3" };
        var json = JsonConvert.SerializeObject(product);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/product", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Product>();
        Assert.Equal("Product3", result?.Name);
    }





    // Put Tests

    [Fact]
    public async Task Put_ValidProduct_ReturnsOk()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Products.RemoveRange(dbContext.Products);
        var product = new Product { Name = "Product1" };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        var id = product.Id;
        product.Name = "UpdatedProduct";
        var json = JsonConvert.SerializeObject(product);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/product/{id}", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Product>();
        Assert.Equal("UpdatedProduct", result?.Name);
    }

    [Fact]
    public async Task Put_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Products.RemoveRange(dbContext.Products);
        await dbContext.SaveChangesAsync();
        var nonExistingId = 999;
        var product = new Product { Name = "NonExistingProduct" };
        var json = JsonConvert.SerializeObject(product);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/product/{nonExistingId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }





    // Patch Tests

    [Fact]
    public async Task Patch_AddProduct_ReturnsOk()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Products.RemoveRange(dbContext.Products);
        await dbContext.SaveChangesAsync();
        var category = new Category { Name = "test" };
        var product = new Product { Name = "Product", Category = category };
        var json = JsonConvert.SerializeObject(product);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PatchAsync("/api/Product", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Product>();
        Assert.Equal("Product", result?.Name);
    }





    // Delete Tests
    [Fact]
    public async Task Delete_ExistingId_ReturnsOk()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Products.RemoveRange(dbContext.Products);
        var product = new Product { Name = "Product1" };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        var id = product.Id;

        // Act
        var response = await _client.DeleteAsync($"/api/product/{id}");

        // Assert
        response.EnsureSuccessStatusCode();
        using var verifyScope = _factory.Services.CreateScope();
        var verifyContext = verifyScope.ServiceProvider.GetRequiredService<DataContext>();
        var deletedProduct = await verifyContext.Products.FindAsync(id);
        Assert.True(deletedProduct?.Deleted);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Products.RemoveRange(dbContext.Products);
        await dbContext.SaveChangesAsync();
        var nonExistingId = 999;

        // Act
        var response = await _client.DeleteAsync($"/api/product/{nonExistingId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}