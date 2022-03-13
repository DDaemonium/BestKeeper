namespace MobileApplication.Service
{
    using Domain.Dto.Request.Order;
    using Domain.Dto.Response.Order;
    using SharedApplicationsData.Service.Identity;

    public class OrderService
    {
        private readonly string _ordersControllerEndpoint;
        private readonly IdentityHttpClient _identityHttpClient;

        public OrderService(IConfiguration configuration, IdentityHttpClient identityHttpClient)
        {
            _ordersControllerEndpoint = $"{configuration["ServerEndpoint"]}{configuration["OrdersController"]}";
            _identityHttpClient = identityHttpClient;
        }

        public async Task<List<OrderInfo>?> GetOpenedOrdersOfCurrentUserAsync()
        => await _identityHttpClient.GetAsync<List<OrderInfo>>($"{_ordersControllerEndpoint}/my/opened");
        public async Task<OrderInfo?> GetOrderByIdAsync(Guid orderId)
        => await _identityHttpClient.GetAsync<OrderInfo>($"{_ordersControllerEndpoint}/{orderId}");
        public async Task CloseOrderAsync(Guid orderId, string reason)
        => await _identityHttpClient.DeleteAsync($"{_ordersControllerEndpoint}/close/{orderId}/{reason}");
        public async Task<OrderInfo?> CreateOrderAsync(NewOrder newOrder)
        => await _identityHttpClient.PostAsync<NewOrder, OrderInfo>($"{_ordersControllerEndpoint}/create", newOrder);
    }
}
