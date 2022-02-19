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

        public async Task Login(LoginInfo loginInfo)
        {
            var jwtToken = await _identityHttpClient.PostAsync<LoginInfo, string>($"{_identityControllerEndpoint}/login", loginInfo);
            if (string.IsNullOrEmpty(jwtToken))
            {
                return;
            }

            _identityManager.Jwt = jwtToken;
        }
    }
}
