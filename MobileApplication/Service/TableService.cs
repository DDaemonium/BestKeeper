namespace MobileApplication.Service
{
    using Domain.Dto.Response.Management;
    using SharedApplicationsData.Service.Identity;

    public class TableService
    {
        private readonly string _tablesControllerEndpoint;
        private readonly IdentityHttpClient _identityHttpClient;

        public TableService(IConfiguration configuration, IdentityHttpClient identityHttpClient)
        {
            _tablesControllerEndpoint = $"{configuration["ServerEndpoint"]}{configuration["TablesController"]}";
            _identityHttpClient = identityHttpClient;
        }

        public async Task<List<TableInfo>?> GetAvailableTablesAsync()
        => await _identityHttpClient.GetAsync<List<TableInfo>>($"{_tablesControllerEndpoint}/available");
    }
}
