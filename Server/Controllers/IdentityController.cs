namespace Server.Controllers
{
    using Domain.Data.Identity;
    using Domain.Dto.Request.Identity;
    using Domain.Dto.Response.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Server.Service.Identity;

    [Route("api/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityService _identityService;
        private readonly JwtService _jwtService;

        public IdentityController(IdentityService identityService, JwtService jwtService)
        {
            _identityService = identityService;
            _jwtService = jwtService;
        }

        [Authorize]
        [HttpGet("token/refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            IdentityUser user;
            try
            {
                user = await _identityService.GetUserByIdAsync(CurrentUserId);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(_jwtService.GetJwt(user));
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInfo loginInfo)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            IdentityUser user;
            try
            {
                user = await _identityService.LoginAsync(loginInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (user is null)
            {
                return BadRequest("Invalid credentials");
            }

            return Ok(_jwtService.GetJwt(user));
        }

        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpPost("users/register")]
        public async Task<IActionResult> RegisterUser(NewUser newUser)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            return Ok(await _identityService.RegisterAsync(newUser));
        }

        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers() => Ok(await _identityService.GetAllUsersAsync());

        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(Guid id) => Ok(await _identityService.GetUserByIdAsync(id));

        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpPatch("users/{id}/activity")]
        public async Task<IActionResult> ChangeUserActivity([FromRoute] Guid id, [FromBody] bool isActive)
        {
            if (id == CurrentUserId)
            {
                return BadRequest("Unable to deactivate current user.");
            }

            return Ok(await _identityService.ChangeUserActivityAsync(id, isActive));
        }

        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpPatch("users/password/reset")]
        public async Task<IActionResult> ResetPassword(ChangeUserPassword changeUserPassword)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            return Ok(await _identityService.ResetPasswordAsync(changeUserPassword));
        }

        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpPut("users/update")]
        public async Task<IActionResult> UpdateUserInfoAsync(UpdateUserInfo updateUserInfo)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessage = string.Join(Environment.NewLine, ModelState.Select(x => x.Value));
                return BadRequest(errorsMessage);
            }

            var roles = new Roles();
            if(updateUserInfo.Id == CurrentUserId && (!updateUserInfo.IsActive || updateUserInfo.RoleId != roles.Administrator.Id))
            {
                return BadRequest("Unable to deactivate current user.");
            }

            return Ok(await _identityService.UpdateUserInfoAsync(updateUserInfo));
        }

        private Guid CurrentUserId => Guid.Parse(HttpContext.User.Claims.First(c => c.Type.Equals("UserId")).Value);
    }
}
