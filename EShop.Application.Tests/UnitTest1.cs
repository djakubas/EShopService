using EShop.Application;

namespace EShop.Application.Tests
{
    public class CreditCarService_Tests
    {
        [Theory]
        [InlineData("3497 7965 8312 797",true)]
        [InlineData("345-470-784-783-010",true)]
        [InlineData("378523393817437",true)]
        [InlineData("3785233938174a7",false)]
        [InlineData("",false)]
        [InlineData("234564",false)]
        [InlineData("2345643456543456765434567654345676543", false)]
        [InlineData("3497 7965 8312 ", false)]
        public void ValidateCardNumber_Tests(string cardNumber,bool expected)
        {
            var result = CreditCardService.ValidateCardNumber(cardNumber);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("3497 7965 8312 797", "American Express")]
        [InlineData("345-470-784-783-010", "American Express")]
        [InlineData("378523393817437", "American Express")]
        [InlineData("4024-0071-6540-1778", "Visa")]
        [InlineData("4532 2080 2150 4434", "Visa")]
        [InlineData("4532289052809181", "Visa")]
        [InlineData("5530016454538418", "MasterCard")]
        [InlineData("5551561443896215", "MasterCard")]
        [InlineData("5131208517986691", "MasterCard")]
        [InlineData("00000000000000", "Unknown")]
        public void GetCardType_Tests(string cardNumber, string expected)
        {
            var result = CreditCardService.GetCardType(cardNumber);
            Assert.Equal(expected, result);
        }
    }
}