namespace WebApplication.Service.Management
{
    using Domain.Dto.Request.Management;
    using Domain.Dto.Response.Management;
    using SharedApplicationsData.Service.Identity;

    public class TableService
    {
        private readonly string _tableesControllerEndpoint;
        private readonly IdentityHttpClient _identityHttpClient;

        public TableService(IConfiguration configuration, IdentityHttpClient identityHttpClient)
        {
            _tableesControllerEndpoint = $"{configuration["ServerEndpoint"]}{configuration["TablesController"]}";
            _identityHttpClient = identityHttpClient;
        }

        public async Task<TableInfo?> GetTableByIdAsync(Guid tableId)
        => await _identityHttpClient.GetAsync<TableInfo>($"{_tableesControllerEndpoint}/{tableId}");

        public async Task<List<TableInfo>?> GetAllTablesAsync()
        => await _identityHttpClient.GetAsync<List<TableInfo>>($"{_tableesControllerEndpoint}");

        public async Task<TableInfo?> CreateTableAsync(NewTable newTable)
        => await _identityHttpClient.PostAsync<NewTable, TableInfo>($"{_tableesControllerEndpoint}/create", newTable);

        public async Task<TableInfo?> UpdateTableAsync(UpdateTable updateTable)
        => await _identityHttpClient.PutAsync<UpdateTable, TableInfo>($"{_tableesControllerEndpoint}/update", updateTable);

        public async Task<bool?> ChangeTableReservation(Guid tableId, bool isReserved)
        => await _identityHttpClient.PatchAsync<bool, bool>($"{_tableesControllerEndpoint}/{tableId}/reservation", isReserved);

        public async Task DeleteTableAsync(Guid dishId)
        => await _identityHttpClient.DeleteAsync($"{_tableesControllerEndpoint}/delete/{dishId}");
    }
}
