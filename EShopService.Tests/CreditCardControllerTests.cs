using EShop.Application.Services;
using EShop.Domain.Exceptions.CreditCard;
using EShopService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EShopService.Tests;

public class CreditCardControllerTests
{
    // GetValidation Tests

    [Fact]
    public void GetValidation_ValidCardNumber_ReturnsOk()
    {
        // Arrange
        var mockService = new Mock<ICreditCardService>();
        mockService.Setup(s => s.ValidateCardNumber("12 1234 1234 1234 1234")).Returns(true);
        var controller = new CreditCardController(mockService.Object);

        // Act
        var result = controller.GetValidation("12 1234 1234 1234 1234");

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetValidation_InvalidCardNumber_ReturnsBadRequest()
    {
        // Arrange
        var mockService = new Mock<ICreditCardService>();
        mockService.Setup(s => s.ValidateCardNumber("12 1234")).Throws(new CardNumberTooShortException());
        var controller = new CreditCardController(mockService.Object);

        // Act & Assert
        var result = controller.GetValidation("12 1234");
        Assert.IsType<BadRequestObjectResult>(result);
    }




    // GetType Tests

    [Fact]
    public void GetType_RecognizedCardNumber_ReturnsOk()
    {
        // Arrange
        var mockService = new Mock<ICreditCardService>();
        mockService.Setup(s => s.GetCardType("12 1234 1234 1234 1234")).Returns("Visa");
        var controller = new CreditCardController(mockService.Object);

        // Act
        var result = controller.GetType("12 1234 1234 1234 1234");

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetType_UnrecognizedCardNumber_ReturnsBadRequest()
    {
        // Arrange
        var mockService = new Mock<ICreditCardService>();
        mockService.Setup(s => s.GetCardType("99 1234 1234 1234 1234")).Throws(new CardNumberInvalidException("Credit Card provider not on the list"));
        var controller = new CreditCardController(mockService.Object);

        // Act
        var result = controller.GetType("99 1234 1234 1234 1234");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void GetType_UnsupportedCardNumber_ReturnsBadRequest()
    {
        // Arrange
        var mockService = new Mock<ICreditCardService>();
        mockService.Setup(s => s.GetCardType("99 1234 1234 1234 1234")).Throws(new CardNumberInvalidException("Credit Card provider not on the list"));
        var controller = new CreditCardController(mockService.Object);

        // Act
        var result = controller.GetType("99 1234 1234 1234 1234");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}