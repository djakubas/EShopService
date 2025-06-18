using EShop.Application.Services;
using EShop.Domain.Exceptions.CreditCard;
using EShopService.Controllers;
using Moq;

namespace EShopService.Tests;

public class CreditCardControllerTests
{
    // GetValidation Tests

    [Fact]
    public void GetValidation_ValidCardNumber_ReturnsTrue()
    {
        // Arrange
        var mockService = new Mock<ICreditCardService>();
        mockService.Setup(s => s.ValidateCardNumber("12 1234 1234 1234 1234")).Returns(true);
        var controller = new CreditCardController(mockService.Object);

        // Act
        var result = controller.GetValidation("12 1234 1234 1234 1234");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetValidation_InvalidCardNumber_ThrowsException()
    {
        // Arrange
        var mockService = new Mock<ICreditCardService>();
        mockService.Setup(s => s.ValidateCardNumber("12 1234")).Throws(new CardNumberTooShortException());
        var controller = new CreditCardController(mockService.Object);

        // Act & Assert
        Assert.ThrowsAny<Exception>(() => controller.GetValidation("12 1234"));
    }




    // GetType Tests

    [Fact]
    public void GetType_RecognizedCardNumber_ReturnsCardType()
    {
        // Arrange
        var mockService = new Mock<ICreditCardService>();
        mockService.Setup(s => s.GetCardType("12 1234 1234 1234 1234")).Returns("Visa");
        var controller = new CreditCardController(mockService.Object);

        // Act
        var result = controller.GetType("12 1234 1234 1234 1234");

        // Assert
        Assert.Equal("Visa", result);
    }

    [Fact]
    public void GetType_UnrecognizedCardNumber_ReturnsUnknown()
    {
        // Arrange
        var mockService = new Mock<ICreditCardService>();
        mockService.Setup(s => s.GetCardType("99 1234 1234 1234 1234")).Returns("Unknown");
        var controller = new CreditCardController(mockService.Object);

        // Act
        var result = controller.GetType("99 1234 1234 1234 1234");

        // Assert
        Assert.Equal("Unknown", result);
    }
}