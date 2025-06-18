using Microsoft.AspNetCore.Mvc;
using EShop.Application;
using EShop.Domain.Models;
using EShop.Application.Services;

namespace EShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController
    {
        private readonly ICreditCardService _creditCardService;
        public CreditCardController(ICreditCardService creditCardService)
        {
            _creditCardService = creditCardService;
        }

        [HttpGet("Validate/{cardNumber}")]
        public bool GetValidation(string cardNumber)
        {
            return _creditCardService.ValidateCardNumber(cardNumber);
        }

        [HttpGet("CardType/{cardNumber}")]
        public string GetType(string cardNumber)
        {
            return _creditCardService.GetCardType(cardNumber);
        }
    }
}
