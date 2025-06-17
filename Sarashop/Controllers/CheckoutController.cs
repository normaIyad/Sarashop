using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Sarashop.DTO;
using Sarashop.Models;
using Sarashop.service;
using Stripe.Checkout;
using System.Security.Claims;

namespace Sarashop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly ICart cartServices;
        private readonly IOrderService orderService;
        private readonly UserManager<ApplecationUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly IOrderItem prodactServise;

        public CheckoutController(ICart cartServices, IOrderService orderService, UserManager<ApplecationUser> userManager,
            IEmailSender emailSender, IOrderItem prodactServise)
        {
            this.cartServices = cartServices;
            this.orderService = orderService;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.prodactServise = prodactServise;
        }
        [HttpGet("pay")]
        public async Task<IActionResult> Pay([FromBody] PaymentRequest paymentRequest)
        {
            var appUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(appUser))
                return Unauthorized("User not found.");
            var cart = await cartServices.GetAsync(e => e.ApplecationUserId == appUser, [e => e.Product]);
            if (cart == null || !cart.Any())
            {
                return BadRequest("Cart is empty.");
            }
            Order order = new Order()
            {
                orderStatus = OrderStatus.pending,
                OrderDate = DateTime.Now,
                TotalPrice = cart.Sum(e => e.Product.Price * e.Count),
                applecationUserId = appUser,
            };
            if (paymentRequest.PaymentMethode == "Cash")
            {
                order.pamentMethodeType = PamentMethodeType.Chash;
                await orderService.AddAsync(order);
                return RedirectToAction(nameof(Success), new { orderid = order.Id });
            }
            if (paymentRequest.PaymentMethode == "Visa")
            {
                order.pamentMethodeType = PamentMethodeType.Visa;
                await orderService.AddAsync(order);
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/api/Checkout/success/{order.Id}",
                    CancelUrl = $"{Request.Scheme}://{Request.Host}/Checkout/cancel" // 🔧 fixed typo: Schreme → Scheme
                };

                foreach (var item in cart)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name,
                                Description = item.Product.Description
                            },
                            UnitAmount = (long)(item.Product.Price * 100) // 🔧 Stripe expects amount in cents
                        },
                        Quantity = item.Count
                    });
                }

                var service = new SessionService();
                var session = await service.CreateAsync(options);
                order.SessionId = session.Id;
                await orderService.commitAsync();
                return Ok(new { session = session.Url });
            }
            else
            {
                return BadRequest(new { massege = "Invalled payment methoud " });
            }


        }

        [HttpGet("success/{orderid}")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromRoute] int orderid)
        {
            var order = await orderService.GetOne(e => e.Id == orderid);
            var user = await userManager.FindByIdAsync(order.applecationUserId);
            var cart = await cartServices.GetAsync(e => e.ApplecationUserId == order.applecationUserId,
                [e => e.Product]);
            List<OrderItem> items = new List<OrderItem>();
            foreach (var item in cart)
            {
                //ProductId
                items.Add(new()
                {
                    OrderId = orderid,
                    ProdactId = item.ProductId,
                    TotalPrice = item.Product.Price * item.Count,
                });
                item.Product.Quntity -= item.Count;
            }
            await prodactServise.AddMnayAsync(items);

            await cartServices.RemoveRangeAsync(cart.ToList());
            await orderService.commitAsync();
            string supject = "";
            if (order.pamentMethodeType == PamentMethodeType.Visa)
            {
                order.orderStatus = OrderStatus.Approved;
                var service = new SessionService();
                var session = service.Get(order.SessionId);
                order.TransactionID = session.Id;
                await orderService.commitAsync();
                supject = "Order resived - visa payment ";

            }
            else
            {
                supject = "Order payment succsess  - chash payment ";

            }
            await emailSender.SendEmailAsync(user.Email, supject, $"<h2>Dear {user.UserName}</h2>" +
                $"your order has pen resives {order.Id}   ");
            return Ok(new { massege = "Done" });
        }


    }
}
