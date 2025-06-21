using Microsoft.AspNetCore.Mvc;
using EShop.Application.Services;
using EShop.Domain.Exceptions;
using EShop.Domain.Exceptions.Products;
using EShop.Domain.Exceptions.Cart;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EShop.Domain.Models.Cart;

namespace EShopService.Controllers
{
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Employee,Client")]
        public async Task<IActionResult> GetCartId()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new UnauthorizedAccessException("User is not authenticated.");
                var cartId = await _cartService.GetCartIdByUserIdAsync(Guid.Parse(userId));
                return Ok(new { CartId = cartId });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("AddItem")]
        [Authorize(Roles = "Administrator,Employee,Client")]
        public async Task<IActionResult> AddItemToCart([FromBody] int productId, int quantity, Guid cartId)
        {
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
            }
            catch (CartNotFoundException e )
            {
                return NotFound(new { message = e.Message });
            }
        }
    }
}
