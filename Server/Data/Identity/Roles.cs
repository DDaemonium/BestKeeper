namespace Server.Data.Identity
{
    using Domain.Data.Identity;

    public class Roles
    {
        public Role Administrator { get; } = new(Guid.Parse("53be8d81-2725-4c34-8d56-6ee936e76c30"), RoleConstants.Administrator);
        public Role Manager { get; } = new(Guid.Parse("43f991a4-5396-4f42-a741-7daa2270d054"), RoleConstants.Manager);
        public Role Waiter { get; } = new(Guid.Parse("53be8d81-2725-4c34-8d56-6ee936e85c30"), RoleConstants.Waiter);
        public Role[] All => new Role[] { Administrator, Manager, Waiter };
    };
}
