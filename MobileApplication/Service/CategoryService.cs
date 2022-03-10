namespace MobileApplication.Service
{
    using Domain.Dto.Response.Dish;
    using SharedApplicationsData.Service.Identity;

    public class CategoryService
    {
        private readonly string _categoriesControllerEndpoint;
        private readonly IdentityHttpClient _identityHttpClient;

        public CategoryService(IConfiguration configuration, IdentityHttpClient identityHttpClient)
        {
            _categoriesControllerEndpoint = $"{configuration["ServerEndpoint"]}{configuration["CategoriesController"]}";
            _identityHttpClient = identityHttpClient;
        }

        public async Task<List<CategoryInfo>?> GetAllCategoriesAsync()
        => await _identityHttpClient.GetAsync<List<CategoryInfo>>($"{_categoriesControllerEndpoint}");
    }
}
