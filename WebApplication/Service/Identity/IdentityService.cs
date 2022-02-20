namespace WebApplication.Service.Identity
{
    using Domain.Dto.Request.Identity;

    public class IdentityService
    {
        private readonly IdentityHttpClient _identityHttpClient;
        private readonly IdentityManager _identityManager;
        private readonly string _identityControllerEndpoint;

        public IdentityService(IdentityHttpClient identityHttpClient, IConfiguration configuration, IdentityManager identityManager)
        {
            _identityHttpClient = identityHttpClient;
            _identityManager = identityManager;
            _identityControllerEndpoint = $"{configuration["ServerEndpoint"]}{configuration["IdentityController"]}";
        }

        public void LogOut()
        {
            _identityManager.Jwt = string.Empty;
        }

        public async Task RefreshTokenAsync()
        {
            var jwtToken = await _identityHttpClient.GetAsync($"{_identityControllerEndpoint}/token/refresh");
            if (string.IsNullOrEmpty(jwtToken))
            {
                return;
            }

            _identityManager.Jwt = jwtToken;
        }

        public async Task LoginAsync(LoginInfo loginInfo)
        {
            var jwtToken = await _identityHttpClient.PostAsync($"{_identityControllerEndpoint}/login", loginInfo);
            if (string.IsNullOrEmpty(jwtToken))
            {
                return;
            }

            _identityManager.Jwt = jwtToken;
        }
    }
}
