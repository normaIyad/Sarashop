using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarashop.DTO;
using Sarashop.Models;
using Sarashop.service;
using System.Security.Claims;

namespace Sarashop.Controllers
{
    [Route("api/prodacts/{prodactID}/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IOrderItem _orderItemService;

        private readonly IReview reviewService;

        public ReviewController(IOrderItem orderItemService, IReview reviewService)
        {
            _orderItemService = orderItemService;
            this.reviewService = reviewService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(int prodactID, [FromForm] ReviewRequest review)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var hasorder = (await _orderItemService.GetAsync(
                e => e.ProdactId == prodactID &&
                e.Order.applecationUserId == user
            )).Any();
            if (hasorder)
            {
                var hasReview = await reviewService.GetAsync(e => e.ProdactId == prodactID && e.ApplecationUserID == user);
                if (hasReview.Any())
                {
                    return BadRequest(new { massege = "You Make review before" });
                }

                else
                {
                    var reviewData = review.Adapt<Review>();
                    reviewData.ProdactId = prodactID;
                    reviewData.ApplecationUserID = user;
                    reviewData.ReviewDate = DateTime.Now;
                    await reviewService.AddAsync(reviewData);
                    return Ok("Done");
                }

            }
            else
            {
                return BadRequest(new { massege = "You cant Review the Prodact " });
            }
            return Ok();
        }
    }
}
