namespace Server.Controllers
{
    using Domain.Data.Identity;
    using Domain.Dto.Request.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Server.Service.Management;

    [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Waiter}")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _orderService.GetOrdersAsync());

        [HttpGet("my")]
        public async Task<IActionResult> GetOrdersForCurrentUser()
            => Ok(await _orderService.GetOrdersOfCurrentUserAsync(CurrentUserId));

        [HttpGet("my/opened")]
        public async Task<IActionResult> GetOpenedOrdersForCurrentUser()
            => Ok(await _orderService.GetOpenedOrdersOfCurrentUserAsync(CurrentUserId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _orderService.GetOrderByIdAsync(id));

        [HttpDelete("close/{id}/{reason}")]
        public async Task<IActionResult> Delete(Guid id, string reason)
        {
            await _orderService.CloseOrderAsync(id, reason);
            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NewOrder newOrder)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            return Ok(await _orderService.CreateOrderAsync(newOrder, CurrentUserId));
        }

        private Guid CurrentUserId => Guid.Parse(HttpContext.User.Claims.First(c => c.Type.Equals("UserId")).Value);
    }
}
