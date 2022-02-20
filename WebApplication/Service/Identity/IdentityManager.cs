namespace WebApplication.Service.Identity
{
    using Blazored.LocalStorage;
    using WebApplication.Data.Identity;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.AspNetCore.Components;

    public class IdentityManager
    {
        private readonly ISyncLocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly JwtSecurityTokenHandler _handler = new();
        private readonly string _loginRedirectPath;
        private const string JwtKey = "JWT";

        public IdentityManager(ISyncLocalStorageService localStorage, IConfiguration configuration, NavigationManager navigationManager)
        {
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _loginRedirectPath = configuration["LoginRedirectPath"];
        }

        public string Jwt
        {
            get => _localStorage.GetItemAsString(JwtKey);
            set => _localStorage.SetItemAsString(JwtKey, value);
        }

        public void RequireAuthorization()
        {
            if(!IsAuthorized)
            {
                _navigationManager.NavigateTo($"{_loginRedirectPath}?returnUrl={Uri.EscapeDataString(_navigationManager.Uri)}");
            }
        }

        public bool IsAuthorized => !string.IsNullOrEmpty(Jwt);

        public Identity Identity => IsAuthorized ? new(_handler.ReadJwtToken(Jwt)) : null;

        public bool IsUserInRole(string role) => IsAuthorized && Identity.Role.Equals(role);
    }
}
