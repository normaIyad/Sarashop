using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sarashop.Models;
using Sarashop.service;

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
        public async Task<IActionResult> AddToCart([FromRoute] int productid, [FromQuery] int count)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var cart = new Cart()
                {
                    ProductId = productid,
                    Count = count,
                    ApplecationUserId = userId
                };
                await _cartServices.AddAsync(cart);
                return Ok(cart);
            }
            return Unauthorized(); // Prefer Unauthorized if user is not authenticated
        }
    }
}
