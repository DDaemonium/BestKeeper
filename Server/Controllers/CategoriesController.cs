namespace Server.Controllers
{
    using Domain.Data.Identity;
    using Domain.Dto.Request.Dish;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Server.Service.Dishes;

    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager},{RoleConstants.Waiter}")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _categoryService.GetAll());

        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager},{RoleConstants.Waiter}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _categoryService.GetCategoryAsync(id));

        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager}")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok();
        }

        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager}")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(NewCategory newCategory)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            return Ok(await _categoryService.AddCategoryAsync(newCategory));
        }

        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager}")]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateCategory updateCategory)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            return Ok(await _categoryService.UpdateCategoryAsync(updateCategory));
        }
    }
}
