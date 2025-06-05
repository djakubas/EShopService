using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using EShop.Domain;
using EShop.Domain.Enums;
using EShop.Domain.Exceptions.CreditCard;


namespace EShop.Application.Services
{
    public class CreditCardService : ICreditCardService
    {
        public bool ValidateCardNumber(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            if (!cardNumber.All(char.IsDigit))
                throw new CardNumberInvalidException();
            else if (cardNumber.Length < 13)
                throw new CardNumberTooShortException();
            else if (cardNumber.Length > 19)
                throw new CardNumberTooLongException();

            //Luhn Algorithm
            int sum = 0;
            bool alternate = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = cardNumber[i] - '0';

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                alternate = !alternate;
            }

            if (!(sum % 10 == 0))
                throw new CardNumberInvalidException();
            return true;

        }

        public string GetCardType(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            if (Regex.IsMatch(cardNumber, @"^4(\d{12}|\d{15}|\d{18})$"))
                return CheckOnProvidersList("Visa");
            else if (Regex.IsMatch(cardNumber, @"^(5[1-5]\d{14}|2(2[2-9][1-9]|2[3-9]\d{2}|[3-6]\d{3}|7([01]\d{2}|20\d))\d{10})$"))
                return CheckOnProvidersList("Mastercard");

            else if (Regex.IsMatch(cardNumber, @"^3[47]\d{13}$"))
                return CheckOnProvidersList("American_Express");

            else if (Regex.IsMatch(cardNumber, @"^(6011\d{12}|65\d{14}|64[4-9]\d{13}|622(1[2-9][6-9]|[2-8]\d{2}|9([01]\d|2[0-5]))\d{10})$"))
                return CheckOnProvidersList("Discover");

            else if (Regex.IsMatch(cardNumber, @"^(352[89]|35[3-8]\d)\d{12}$"))
                return CheckOnProvidersList("JCB");

            else if (Regex.IsMatch(cardNumber, @"^3(0[0-5]|[68]\d)\d{11}$"))
                return CheckOnProvidersList("Diners_Club");

            else if (Regex.IsMatch(cardNumber, @"^(50|5[6-9]|6\d)\d{10,17}$"))
                return CheckOnProvidersList("Maestro");

            return "Unknown";
        }
        private string CheckOnProvidersList(string cardProvider)
        {
            if (!Enum.IsDefined(typeof(CreditCardProvider), cardProvider))
                throw new CardNumberInvalidException("Credit Card provider not on the list");
            return cardProvider;
        }

    }

    public interface ICreditCardService
    {
        public bool ValidateCardNumber(string cardNumber);
        public string GetCardType(string cardNumber);
    }
}
