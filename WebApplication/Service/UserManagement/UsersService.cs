namespace WebApplication.Service.UserManagement
{
    using Domain.Dto.Request.Identity;
    using Domain.Dto.Response.Identity;
    using WebApplication.Service.Identity;

    public class UsersService
    {
        private readonly string _identityControllerEndpoint;
        private readonly IdentityHttpClient _identityHttpClient;
        private readonly IdentityService _identityService;
        private readonly IdentityManager _identityManager;

        public UsersService(IConfiguration configuration, IdentityHttpClient identityHttpClient, IdentityService identityService, IdentityManager identityManager)
        {
            _identityControllerEndpoint = $"{configuration["ServerEndpoint"]}{configuration["IdentityController"]}";
            _identityHttpClient = identityHttpClient;
            _identityService = identityService;
            _identityManager = identityManager;
        }

        public async Task<List<IdentityUser>?> GetAllUsersAsync()
        => await _identityHttpClient.GetAsync<List<IdentityUser>>($"{_identityControllerEndpoint}/users");

        public async Task<IdentityUser?> GetUserByIdAsync(Guid id)
        => await _identityHttpClient.GetAsync<IdentityUser>($"{_identityControllerEndpoint}/users/{id}");

        public async Task<ResetPasswrdMessage?> ResetPasswordAsync(ChangeUserPassword changeUserPassword)
        => await _identityHttpClient.PatchAsync<ChangeUserPassword, ResetPasswrdMessage>($"{_identityControllerEndpoint}/users/password/reset", changeUserPassword);
        
        public async Task<IdentityUser?> RegisterAsync(NewUser newUser)
        => await _identityHttpClient.PostAsync<NewUser, IdentityUser>($"{_identityControllerEndpoint}/users/register", newUser);

        public async Task<IdentityUser?> UpdateUserInfoAsync(UpdateUserInfo updateUserInfo)
        {
            var user = await _identityHttpClient.PutAsync<UpdateUserInfo, IdentityUser>($"{_identityControllerEndpoint}/users/update", updateUserInfo);
            if (user is not null && _identityManager.Identity.Id == user.Id)
            {
                await _identityService.RefreshTokenAsync();
            }

            return user;
        }

    }
}
