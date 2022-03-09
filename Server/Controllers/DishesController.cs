namespace Server.Controllers
{
    using Domain.Data.Identity;
    using Domain.Dto.Request.Dish;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Server.Service.Dishes;

    [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager}")]
    [Route("api/dishes")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly DishService _dishService;

        public DishesController(DishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _dishService.GetDishesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _dishService.GetDishAsync(id));

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _dishService.DeleteDishAsync(id);
            return Ok();
        }

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
