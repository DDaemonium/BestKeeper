namespace WebApplication.Service.Identity
{
    using Blazored.LocalStorage;
    using WebApplication.Data.Identity;
    using System.IdentityModel.Tokens.Jwt;

    public class IdentityManager
    {
        private readonly ISyncLocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler _handler = new();
        private const string JwtKey = "JWT";

        public IdentityManager(ISyncLocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public string Jwt
        {
            get => _localStorage.GetItemAsString(JwtKey);
            set => _localStorage.SetItemAsString(JwtKey, value);
        }

        public bool IsAuthorized => !string.IsNullOrEmpty(Jwt);

        public Identity Identity => IsAuthorized ? new(_handler.ReadJwtToken(Jwt)) : null;

        public bool IsUserInRole(string role) => IsAuthorized && Identity.Role.Equals(role);
    }
}
