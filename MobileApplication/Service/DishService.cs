namespace MobileApplication.Service
{
    using Domain.Dto.Response.Dish;
    using SharedApplicationsData.Service.Identity;

    public class DishService
    {
        private readonly string _dishesControllerEndpoint;
        private readonly IdentityHttpClient _identityHttpClient;

        public DishService(IConfiguration configuration, IdentityHttpClient identityHttpClient)
        {
            _dishesControllerEndpoint = $"{configuration["ServerEndpoint"]}{configuration["DishesController"]}";
            _identityHttpClient = identityHttpClient;
        }

        public async Task<DishInfo?> GetDishByIdAsync(Guid dishId)
        => await _identityHttpClient.GetAsync<DishInfo>($"{_dishesControllerEndpoint}/{dishId}");

        public async Task<List<DishInfo>?> GetAllDishesAsync()
        => await _identityHttpClient.GetAsync<List<DishInfo>>($"{_dishesControllerEndpoint}");
    }
}
