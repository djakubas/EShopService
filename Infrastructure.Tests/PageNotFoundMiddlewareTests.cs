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
public class PageNotFoundMiddlewareTests
{

    //deprecated
    //[Fact]
    //public async Task InvokeAsync_Status404_SetsJsonContentType()
    //{
    //    // Arrange
    //    var mockNext = new Mock<RequestDelegate>();
    //    var mockContext = new DefaultHttpContext();
    //    mockContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
    //    var middleware = new PageNotFoundMiddleware(mockNext.Object);

    //    // Act
    //    await middleware.InvokeAsync(mockContext);

    //    // Assert
    //    Assert.Equal("application/json", mockContext.Response.ContentType);
    //}

    [Fact]
    public async Task InvokeAsync_Status404_WritesNotFoundMessage()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        var mockContext = new DefaultHttpContext();
        mockContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        using var memoryStream = new MemoryStream();
        mockContext.Response.Body = memoryStream;
        var middleware = new PageNotFoundMiddleware(mockNext.Object);

        // Act
        await middleware.InvokeAsync(mockContext);

        // Assert
        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream);
        var json = reader.ReadToEnd();
        Assert.Contains("What you are looking for is not here", json);
    }

    [Fact]
    public async Task InvokeAsync_NoStatus404_CallsNextDelegate()
    {
        // Arrange
        bool isNextCalled = false;
        var mockNext = new Mock<RequestDelegate>();
        mockNext.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Callback<HttpContext>(context => isNextCalled = true);
        var mockContext = new DefaultHttpContext();
        mockContext.Response.StatusCode = (int)HttpStatusCode.OK;
        var middleware = new PageNotFoundMiddleware(mockNext.Object);

        // Act
        await middleware.InvokeAsync(mockContext);

        // Assert
        Assert.True(isNextCalled);
    }
}
