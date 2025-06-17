using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sarashop.DTO;
using Sarashop.Models;
using Sarashop.service;
using System.Security.Claims;

namespace Sarashop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly CartServices _cartServices;
        private readonly UserManager<ApplecationUser> _userManager;

        public CartsController(CartServices cartServices, UserManager<ApplecationUser> userManager)
        {
            _cartServices = cartServices;
            _userManager = userManager;
        }
        [HttpPost("{productid}")]
        public async Task<IActionResult> AddToCart([FromRoute] int productid, CancellationToken cancellationToken)
        {
            var appUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _cartServices.AddToCart(appUser, productid, cancellationToken);
            return Ok(result);
        }
        //[HttpGet("getUserCart")]
        //public async Task<IActionResult> Get()
        //{
        //    var appUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    var cartItems = await _cartServices.ShowUserCart(appUser);
        //    var cartResponse = cartItems.Select(e => e.Product).Adapt<IEnumerable<CartRequest>>();
        //    return Ok(cartItems);
        //}
        [HttpGet("getUserCart")]
        public async Task<IActionResult> Get()
        {
            var appUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(appUser))
                return Unauthorized("User ID not found.");

            var cartItems = await _cartServices.ShowUserCart(appUser);
            if (!cartItems.Any())
                return NotFound("No cart items found.");

            // Test with raw return first
            // return Ok(cartItems);

            var cartRequest = cartItems.Select(e => e.Product).Adapt<IEnumerable<CartRequest>>();
            var totalPrice = cartItems.Sum(e => e.Product.Price * e.Count);

            return Ok(new { cartRequest, totalPrice });
        }


        //[HttpPost("{productid}")]
        //public async Task<IActionResult> AddToCart([FromRoute] int productid, CancellationToken cancellationToken)
        //{
        //    var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
        //    //User.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
        //    var userId = subClaim.Value;
        //    var result = await _cartServices.AddToCart(userId, productid, cancellationToken);
        //    return Ok(result);
        //}
    }
}
