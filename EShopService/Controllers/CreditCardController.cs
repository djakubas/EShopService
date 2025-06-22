using Microsoft.AspNetCore.Mvc;
using EShop.Application;
using EShop.Domain.Models;
using EShop.Application.Services;

namespace EShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    { 
        private readonly ICreditCardService _creditCardService;
        public CreditCardController(ICreditCardService creditCardService)
        {
            _creditCardService = creditCardService;
        }

        [HttpGet("Validate/{cardNumber}")]
        public IActionResult GetValidation(string cardNumber)
        {
            try
            {
                var result = _creditCardService.ValidateCardNumber(cardNumber);
                return Ok(new { result });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("CardType/{cardNumber}")]
        public IActionResult GetType(string cardNumber)
        {
            try
            {
                var result = _creditCardService.GetCardType(cardNumber);
                return Ok(new { cardProvider = result });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
