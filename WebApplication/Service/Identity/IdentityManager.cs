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

        /// <summary>
        /// Check if user authorized otherwise redirect to login page.
        /// </summary>
        /// <param name="roles">Availabel roles.</param>
        /// <returns>true if user is authorized otherwise false.</returns>
        public bool CheckAuthorizationWithRedirect(params string[] roles)
        {
            if(!IsUserInRole(roles))
            {
                _navigationManager.NavigateTo($"{_loginRedirectPath}?returnUrl={Uri.EscapeDataString(_navigationManager.Uri)}");
                return false;
            }

            return true;
        }

        public bool IsAuthorized => !string.IsNullOrEmpty(Jwt);

        public Identity Identity => IsAuthorized ? new(_handler.ReadJwtToken(Jwt)) : null;

        public bool IsUserInRole(params string[] roles) => !roles.Any() || (IsAuthorized && roles.Contains(Identity.Role));
    }
}
