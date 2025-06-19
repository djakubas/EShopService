using Infrastructure.Middleware;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests;
public class ServiceUnderMaintenanceMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_MaintenanceMode_SetsStatusCode503()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        var mockContext = new DefaultHttpContext();
        var middleware = new ServiceUnderMaintenanceMiddleware(mockNext.Object, true);

        // Act
        await middleware.InvokeAsync(mockContext);

        // Assert
        Assert.Equal((int)HttpStatusCode.ServiceUnavailable, mockContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_MaintenanceMode_SetsJsonContentType()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        var mockContext = new DefaultHttpContext();
        var middleware = new ServiceUnderMaintenanceMiddleware(mockNext.Object, true);

        // Act
        await middleware.InvokeAsync(mockContext);

        // Assert
        Assert.Equal("application/json", mockContext.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_MaintenanceMode_WritesMaintenanceMessage()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        var mockContext = new DefaultHttpContext();
        var middleware = new ServiceUnderMaintenanceMiddleware(mockNext.Object, true);
        using var memoryStream = new MemoryStream();
        mockContext.Response.Body = memoryStream;

        // Act
        await middleware.InvokeAsync(mockContext);

        // Assert
        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream);
        var json = reader.ReadToEnd();
        Assert.Contains("Service under maintenance", json);
    }

    [Fact]
    public async Task InvokeAsync_NoMaintenanceMode_CallsNextDelegate()
    {
        // Arrange
        bool isNextCalled = false;
        var mockNext = new Mock<RequestDelegate>();
        mockNext.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Callback<HttpContext>(context => isNextCalled = true);
        var mockContext = new DefaultHttpContext();
        var middleware = new ServiceUnderMaintenanceMiddleware(mockNext.Object, false);

        // Act
        await middleware.InvokeAsync(mockContext);

        // Assert
        Assert.True(isNextCalled);
    }
}
