using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Orders.Controllers
{

    [ApiController]
    [Route("api/orders")]
    public class OrdersController : Controller
    {

        private readonly IOrdersProvider ordersrProvider;

        public OrdersController(IOrdersProvider ordersrProvider)
        {
            this.ordersrProvider = ordersrProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var result = await ordersrProvider.GetOrdersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Orders);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            var result = await ordersrProvider.GetOrderAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Order);
            }
            return NotFound();
        }
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerOrdersAsync(int customerId)
        {
            var result = await ordersrProvider.GetCustomerOrdersAsync(customerId);
            if (result.IsSuccess)
            {
                return Ok(result.Orders);
            }
            return NotFound();
        }

    }
}
