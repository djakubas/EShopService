using Microsoft.AspNetCore.Mvc;
using EShop.Application;
using EShopService.Models;
namespace EShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController
    {
        [HttpGet("Validate/{cardNumber}")]
        public bool GetValidation(string cardNumber)
        {
            return CreditCardService.ValidateCardNumber(cardNumber);
        }

        [HttpGet("CardType/{cardNumber}")]
        public string GetType(string cardNumber)
        {
            return CreditCardService.GetCardType(cardNumber);
        }


    }
}
