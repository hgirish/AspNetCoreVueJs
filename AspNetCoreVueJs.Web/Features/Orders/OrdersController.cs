using AspNetCoreVueJs.Web.Data;
using AspNetCoreVueJs.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreVueJs.Web.Features.Orders
{
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly IConfiguration _configuration;

        public OrdersController(EcommerceContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpGet("StripePublishKey")]
        public IActionResult GetStripePublishKey()
        {
            var key = _configuration["Stripe:PublishKey"];

            return Ok(new { StripePublishKey = key });
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var orders = await _context.Orders
                .Where(x => x.User.UserName == User.Identity.Name)
                .Select(x => new OrderListViewModel
                {
                    Id = x.Id,
                    Placed = x.Placed,
                    Items = x.Items.Sum(i => i.Quantity),
                    Total = x.Items.Sum(i => i.ProductVariant.Price * i.Quantity),
                    PaymentStatus = Enum.GetName(typeof(PaymentStatus), x.PaymentStatus)
                }).ToListAsync();

            return Ok(orders);
        }

        [HttpPost, Authorize(Roles ="Customer")]
        public async Task<IActionResult> Create([FromBody] CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleAsync(x => x.UserName == HttpContext.User.Identity.Name);

            var order = new Order
            {
                DeliveryAddress = new Address
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    TownCity = model.TownCity,
                    County = model.County,
                    Postcode = model.Postcode
                },
                Items = model.Items.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    ColourId = x.ColourId,
                    StorageId = x.StorageId,
                    Quantity = x.Quantity
                }).ToList()
            };

            user.Orders.Add(order);

            await _context.SaveChangesAsync();

            var total = await _context.Orders
                .Where(x => x.Id == order.Id)
                .Select(x => Convert.ToInt32(x.Items.Sum(i =>
                 i.ProductVariant.Price * i.Quantity) * 100)).SingleAsync();
           
            var charges = new Stripe.ChargeService();
            var charge = await charges.CreateAsync(new Stripe.ChargeCreateOptions
            {
                Amount = total,
                Description = $"Order {order.Id} payment",
                Currency = "GBP",
                SourceId = model.StripeToken
            });

            if (string.IsNullOrEmpty(charge.FailureCode))
            {
                order.PaymentStatus = PaymentStatus.Paid;
            }
            else
            {
                order.PaymentStatus = PaymentStatus.Declined;
            }

            await _context.SaveChangesAsync();



            return Ok(new CreateOrderResponseViewModel(order.Id, order.PaymentStatus));
        }
    }
}
