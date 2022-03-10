namespace WebApplication.Service.DishManagement
{
    using Domain.Dto.Request.Dish;
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

        public async Task<DishInfo?> CreateDishAsync(NewDish newDish)
        => await _identityHttpClient.PostAsync<NewDish, DishInfo>($"{_dishesControllerEndpoint}/create", newDish);

        public async Task<DishInfo?> UpdateDishAsync(UpdateDish updateDish)
        => await _identityHttpClient.PutAsync<UpdateDish, DishInfo>($"{_dishesControllerEndpoint}/update", updateDish);

        public async Task DeleteDishAsync(Guid dishId)
        => await _identityHttpClient.DeleteAsync($"{_dishesControllerEndpoint}/delete/{dishId}");
    }
}
