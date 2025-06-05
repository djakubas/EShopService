using EShop.Application.Services;
using EShop.Domain.Exceptions.CreditCard;

namespace EShop.Application.Tests
{
    public class CreditCardService_Tests
    {
        [Theory]
        [InlineData("3497 7965 8312 797", true)]
        [InlineData("345-470-784-783-010", true)]
        [InlineData("378523393817437", true)]
        public void ValidateCardNumber_Tests_True(string cardNumber, bool expected)
        {
            var _creditCardService = new CreditCardService();
            var result = _creditCardService.ValidateCardNumber(cardNumber);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ValidateCardNumber_Tests_CardNumberTooLongException()
        {
            var _creditCardService = new CreditCardService();
            Assert.Throws<CardNumberTooLongException>(() => _creditCardService.ValidateCardNumber("2345643456543456765434567654345676543"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("234564")]
        [InlineData("3497 7965 8312 ")]
        public void ValidateCardNumber_Tests_CardNumberTooShortException(string cardNumber)
        {
            var _creditCardService = new CreditCardService();
            Assert.Throws<CardNumberTooShortException>(() => _creditCardService.ValidateCardNumber(cardNumber));
        }

        [Fact]
        public void ValidateCardNumber_Tests_CardNumberInvalidException()
        {
            var _creditCardService = new CreditCardService();
            Assert.Throws<CardNumberInvalidException>(() => _creditCardService.ValidateCardNumber("3785233938174a7"));
        }

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
        public void GetCardType_Tests(string cardNumber, string expected)
        {
            var _creditCardService = new CreditCardService();
            var result = _creditCardService.GetCardType(cardNumber);
            Assert.Equal(expected, result);
        }
    }
}