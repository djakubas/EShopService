using EShop.Application.Services;
using EShop.Domain.Exceptions.CreditCard;

namespace EShop.Application.Tests
{
    public class CreditCardServiceTests
    {
        // ValidateCardNumber Tests

        [Theory]
        [InlineData("3497 7965 8312 797", true)]
        [InlineData("345-470-784-783-010", true)]
        [InlineData("378523393817437", true)]
        public void ValidateCardNumber_WhenLuhnIsPassed_ReturnsTrue(string cardNumber, bool expected)
        {
            // Arrange
            var _creditCardService = new CreditCardService();

            // Act
            var result = _creditCardService.ValidateCardNumber(cardNumber);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ValidateCardNumber_WhenTooLong_ThrowsCardNumberTooLongException()
        {
            // Arrange
            var _creditCardService = new CreditCardService();

            // Act & Assert
            Assert.Throws<CardNumberTooLongException>(() => _creditCardService.ValidateCardNumber("2345643456543456765434567654345676543"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("234564")]
        [InlineData("3497 7965 8312 ")]
        public void ValidateCardNumber_WhenTooShort_ThrowsCardNumberTooShortException(string cardNumber)
        {
            // Arrange
            var _creditCardService = new CreditCardService();

            // Act & Assert
            Assert.Throws<CardNumberTooShortException>(() => _creditCardService.ValidateCardNumber(cardNumber));
        }

        [Fact]
        public void ValidateCardNumber_WhenContainsNonDigit_ThrowsCardNumberInvalidException()
        {
            // Arrange
            var _creditCardService = new CreditCardService();

            // Act & Assert
            Assert.Throws<CardNumberInvalidException>(() => _creditCardService.ValidateCardNumber("4532289052809181a"));
        }





        // GetCardType Tests

        [Theory]
        [InlineData("3497 7965 8312 797", "American_Express")]
        [InlineData("345-470-784-783-010", "American_Express")]
        [InlineData("378523393817437", "American_Express")]
        [InlineData("4024-0071-6540-1778", "Visa")]
        [InlineData("4532 2080 2150 4434", "Visa")]
        [InlineData("4532289052809181", "Visa")]
        [InlineData("5530016454538418", "Mastercard")]
        [InlineData("5551561443896215", "Mastercard")]
        [InlineData("5131208517986691", "Mastercard")]
        [InlineData("00000000000000", "Unknown")]
        public void GetCardType_WhenGivenValidCardNumber_ReturnsCorrespondingProvider(string cardNumber, string expected)
        {
            // Arrange
            var _creditCardService = new CreditCardService();

            // Act
            var result = _creditCardService.GetCardType(cardNumber);

            // Assert
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("6011123456789012")] // Discover Card
        [InlineData("3530123456789012")] // JCB Card
        [InlineData("30512345678901")] // Diners Club Card
        [InlineData("5012345678901234")] // Maestro Card
        public void GetCardType_WhenProviderNotSupported_ThrowsCardNumberInvalidException(string cardNumber)
        {
            // Arrange
            var _creditCardService = new CreditCardService();

            // Act & Assert 
            Assert.Throws<CardNumberInvalidException>(() => _creditCardService.ValidateCardNumber(cardNumber));
        }
    }
}