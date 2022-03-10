namespace Server.Controllers
{
    using Domain.Data.Identity;
    using Domain.Dto.Request.Management;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Server.Service.Management;

    [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Manager}")]
    [Route("api/tables")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly TableService _tableService;

        public TablesController(TableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _tableService.GetTablesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _tableService.GetTableAsync(id));

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _tableService.DeleteTableAsync(id);
            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(NewTable newTable)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            return Ok(await _tableService.AddTableAsync(newTable));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateTable updateTable)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            return Ok(await _tableService.UpdateTableAsync(updateTable));
        }

        [HttpPatch("{id}/reservation")]
        public async Task<IActionResult> ChangeUserActivity([FromRoute] Guid id, [FromBody] bool isReserved)
        {
            return Ok(await _tableService.ChangeTableReservation(id, isReserved));
        }
    }
}
