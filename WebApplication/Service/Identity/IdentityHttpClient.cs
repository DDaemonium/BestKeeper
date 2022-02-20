namespace WebApplication.Service.Identity
{
    using System.Net.Http.Json;
    using System.Text;
    using System.Text.Json;

    public class IdentityHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IdentityManager _identityService;

        public IdentityHttpClient(HttpClient httpClient, IdentityManager identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task<T?> GetAsync<T>(string? requestUri)
        {
            AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<T>(requestUri);
        }

        public async Task<string?> GetAsync(string? requestUri)
        {
            AddAuthorizationHeader();
            var requestResult = await _httpClient.GetAsync(requestUri);
            if (requestResult.IsSuccessStatusCode)
            {
                return (await requestResult.Content.ReadFromJsonAsync<ActionResult>())?.Result;
            }

            return default;
        }

        public async Task<TResult?> PostAsync<TRequest, TResult>(string? requestUri, TRequest value)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            var requestResult = await _httpClient.PostAsync(requestUri, content);
            if(requestResult.IsSuccessStatusCode)
            {
                return await requestResult.Content.ReadFromJsonAsync<TResult>();
            }

            return default;
        }

        public async Task<string?> PostAsync<TRequest>(string? requestUri, TRequest value)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            var requestResult = await _httpClient.PostAsync(requestUri, content);
            if(requestResult.IsSuccessStatusCode)
            {
                return (await requestResult.Content.ReadFromJsonAsync<ActionResult>())?.Result;
            }

            return default;
        }

        public async Task<TResult?> PutAsync<TRequest, TResult>(string? requestUri, TRequest value)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            var requestResult = await _httpClient.PutAsync(requestUri, content);
            if (requestResult.IsSuccessStatusCode)
            {
                return await requestResult.Content.ReadFromJsonAsync<TResult>();
            }

            return default;
        }

        public async Task<TResult?> PatchAsync<TRequest, TResult>(string? requestUri, TRequest value)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            var requestResult = await _httpClient.PatchAsync(requestUri, content);
            if (requestResult.IsSuccessStatusCode)
            {
                return await requestResult.Content.ReadFromJsonAsync<TResult>();
            }

            return default;
        }

        public async Task<T?> DeleteAsync<T>(string? requestUri)
        {
            AddAuthorizationHeader();
            var requestResult = await _httpClient.DeleteAsync(requestUri);
            if (requestResult.IsSuccessStatusCode)
            {
                return await requestResult.Content.ReadFromJsonAsync<T>();
            }

            return default;
        }

        public async Task DeleteAsync(string? requestUri)
        {
            AddAuthorizationHeader();
            await _httpClient.DeleteAsync(requestUri);
        }

        private void AddAuthorizationHeader()
        {
            if (!_identityService.IsAuthorized)
            {
                return;
            }

            if(_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_identityService.Jwt}");
        }

        private class ActionResult
        {
            public string? Result { get; set; }
        }
    }
}
