using Infrastructure.Middleware;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Infrastructure.Tests;

public class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_NoException_CallsNextDelegate()
    {
        // Arrange
        bool isNextCalled = false;
        var mockNext = new Mock<RequestDelegate>();
        mockNext.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Callback<HttpContext>(context => isNextCalled = true);
        var mockContext = new DefaultHttpContext();
        var middleware = new ExceptionHandlingMiddleware(mockNext.Object);

        // Act
        await middleware.InvokeAsync(mockContext);

        // Assert
        Assert.True(isNextCalled);
    }

    [Fact]
    public async Task InvokeAsync_WithException_SetsInternalServerError()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        var mockContext = new DefaultHttpContext();
        mockNext.Setup(n => n.Invoke(mockContext)).ThrowsAsync(new Exception("Test exception"));
        var middleware = new ExceptionHandlingMiddleware(mockNext.Object);

        // Act
        await middleware.InvokeAsync(mockContext);

        // Assert
        Assert.Equal((int)HttpStatusCode.InternalServerError, mockContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WithException_SetsJsonContentType()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        var mockContext = new DefaultHttpContext();
        mockNext.Setup(n => n.Invoke(mockContext)).ThrowsAsync(new Exception("Test exception"));
        var middleware = new ExceptionHandlingMiddleware(mockNext.Object);

        // Act
        await middleware.InvokeAsync(mockContext);

        // Assert
        Assert.Equal("application/json", mockContext.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_WithException_WritesJsonResponse()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        var mockContext = new DefaultHttpContext();
        var exceptionMessage = "Test exception";
        mockNext.Setup(n => n.Invoke(mockContext)).ThrowsAsync(new Exception(exceptionMessage));
        var middleware = new ExceptionHandlingMiddleware(mockNext.Object);
        using var memoryStream = new MemoryStream();
        mockContext.Response.Body = memoryStream;

        // Act
        await middleware.InvokeAsync(mockContext);

        // Assert
        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream);
        var json = reader.ReadToEnd();
        Assert.Contains(exceptionMessage, json);
    }
}