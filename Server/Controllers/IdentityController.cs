namespace Server.Controllers
{
    using Domain.Dto.Request.Identity;
    using Domain.Dto.Response.Identity;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInfo loginInfo)
        {
            if (!IsUserValid(loginInfo))
            {
                return BadRequest();
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

        private bool IsUserValid(LoginInfo loginInfo)
        => loginInfo is not null
            && !string.IsNullOrEmpty(loginInfo.Email)
            && !string.IsNullOrEmpty(loginInfo.Password);
    }
}
