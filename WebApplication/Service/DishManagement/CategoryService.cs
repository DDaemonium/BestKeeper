namespace WebApplication.Service.DishManagement
{
    using Domain.Dto.Request.Dish;
    using Domain.Dto.Response.Dish;
    using WebApplication.Service.Identity;

    public class CategoryService
    {
        private readonly string _categoriesControllerEndpoint;
        private readonly IdentityHttpClient _identityHttpClient;

        public CategoryService(IConfiguration configuration, IdentityHttpClient identityHttpClient)
        {
            _categoriesControllerEndpoint = $"{configuration["ServerEndpoint"]}{configuration["CategoriesController"]}";
            _identityHttpClient = identityHttpClient;
        }

        public async Task<CategoryInfo?> GetCategoryByIdAsync(Guid categoryId)
        => await _identityHttpClient.GetAsync<CategoryInfo>($"{_categoriesControllerEndpoint}/{categoryId}");

        public async Task<List<CategoryInfo>?> GetAllCategoriesAsync()
        => await _identityHttpClient.GetAsync<List<CategoryInfo>>($"{_categoriesControllerEndpoint}");

        public async Task<CategoryInfo?> CreateCategoryAsync(NewCategory newCategory)
        => await _identityHttpClient.PostAsync<NewCategory, CategoryInfo>($"{_categoriesControllerEndpoint}/create", newCategory);
        
        public async Task<CategoryInfo?> UpdateCategoryAsync(UpdateCategory updateCategory)
        => await _identityHttpClient.PutAsync<UpdateCategory, CategoryInfo>($"{_categoriesControllerEndpoint}/update", updateCategory);

        public async Task DeleteCategoryAsync(Guid categoryId)
        => await _identityHttpClient.DeleteAsync($"{_categoriesControllerEndpoint}/delete/{categoryId}");
    }
}
