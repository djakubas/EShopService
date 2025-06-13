using Microsoft.AspNetCore.Mvc;
using EShop.Application.Services;
using EShop.Domain.Exceptions;
using EShop.Domain.Exceptions.Products;
using EShop.Domain.Exceptions.Cart;

namespace EShopService.Controllers
{
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItemToCart([FromBody] int productId, int quantity, Guid cartId)
        {
            //dorobic quantity 
            //do rozwazenia czy Guid nie brac przypadkiem z user context? 

            try
            {
                var cart = await _cartService.AddItemToCartAsync(productId, cartId);
                return Ok(cart);
            }
            catch(ProductNotFoundException e)
            {
                return NotFound(new { message = e.Message } );
            }
            catch (CartNotFoundException e )
            {
                return NotFound(new { message = e.Message });
            }
            
        }
        [HttpPost("RemoveItem/{productId}")]
        public async Task<IActionResult> RemoveItemFromCart(int productId, Guid cartId)
        {
            try
            {
                var cart = await _cartService.RemoveItemFromCartAsync(productId, cartId);
                return Ok(cart);
            }
            catch(ProductNotFoundException e)
            {
                return NotFound(new { message = e.Message } );
            }
            catch (CartNotFoundException e )
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet("Calculate")]
        public async Task<IActionResult> CalculateCart(Guid cartId)
        {
            try
            {
                var cart = await _cartService.CalculateCartAsync(cartId);
                return Ok(cart);
            }s
            catch (CartNotFoundException e )
            {
                return NotFound(new { message = e.Message });
            }
        }
    }
}
