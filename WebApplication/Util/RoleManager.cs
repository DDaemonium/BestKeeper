namespace WebApplication.Util
{
    using Domain.Data.Identity;

    public static class RoleManager
    {
        private const string AdministratorViewName = "Администратор";
        private const string ManagerViewName = "Менеджер";
        private const string WaiterViewName = "Официант";
        private static Roles _roles = new();
        public static string GetRoleViewName(string role) => role switch 
        {
            RoleConstants.Administrator => AdministratorViewName,
            RoleConstants.Manager => ManagerViewName,
            RoleConstants.Waiter => WaiterViewName,
            _ => throw new Exception($"Ivalid role name. role: {role}")
        };

        public static string GetRole(string roleViewName) => roleViewName switch 
        {
            AdministratorViewName => RoleConstants.Administrator,
            ManagerViewName => RoleConstants.Manager,
            WaiterViewName => RoleConstants.Waiter,
            _ => throw new Exception($"Ivalid role view name. role: {roleViewName}")
        };

        public static Guid GetRoleId(string roleViewName)
        {
            string roleName = GetRole(roleViewName);
            return roleName switch
            {
                RoleConstants.Administrator => _roles.Administrator.Id,
                RoleConstants.Manager => _roles.Manager.Id,
                RoleConstants.Waiter => _roles.Waiter.Id,
                _ => Guid.Empty,
            };
        }
    }
}
