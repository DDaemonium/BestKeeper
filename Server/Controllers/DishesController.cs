namespace Server.Controllers
{
    using Domain.Data.Identity;
    using Domain.Dto.Request.Dish;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Server.Service.Dishes;

    [Route("api/dishes")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly DishService _dishService;

        public DishesController(DishService dishService)
        {
            _dishService = dishService;
        }

        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager},{RoleConstants.Waiter}")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _dishService.GetDishesAsync());

        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager},{RoleConstants.Waiter}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _dishService.GetDishAsync(id));

        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager}")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _dishService.DeleteDishAsync(id);
            return Ok();
        }

        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager}")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(NewDish newDIsh)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            return Ok(await _dishService.AddDishAsync(newDIsh));
        }

        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager}")]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateDish updateDish)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            return Ok(await _dishService.UpdateDishAsync(updateDish));
        }
    }
}
