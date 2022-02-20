namespace Server.DataAccess
{
    using Domain.Data.Identity;
    using Microsoft.EntityFrameworkCore;
    using Server.DataAccess.Models;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected DatabaseContext()
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishCategory> DishCategories { get; set; }
        public DbSet<DishOrder> DishOrders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var roles = new Roles();

            modelBuilder.Entity<ApplicationUser>(e => {
                e.HasKey(k => k.Id);
                e.Property(p => p.Name).HasMaxLength(256).IsRequired();
                e.Property(p => p.Surname).HasMaxLength(256).IsRequired();
                e.Property(p => p.PhoneNumber).HasMaxLength(256).IsRequired();
                e.Property(p => p.Email).IsRequired();
                e.Property(p => p.Password).IsRequired();

                e.HasOne(u => u.ApplicationUserRole).WithMany(r => r.ApplicationUsers)
                .HasForeignKey(up => up.ApplicationUserRoleId).OnDelete(DeleteBehavior.NoAction);

                // Default user
                e.HasData(new ApplicationUser 
                { 
                    Id = Guid.Parse("53be8d81-2725-4c34-8d56-6ee936e76c31"),
                    Name = "Admin", 
                    Surname = "Admin" ,
                    PhoneNumber = "88005553535",
                    Email = "admin@admin.admin",
                    Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", // admin
                    ApplicationUserRoleId = roles.Administrator.Id,
                    IsActive = true,
                });

            });

            modelBuilder.Entity<ApplicationUserRole>(e => {
                e.HasKey(k =>k.Id);
                e.Property(p => p.Name).HasMaxLength(256).IsRequired();

                e.HasData(roles.All.Select(r => new ApplicationUserRole { Id = r.Id, Name = r.Name }));
            });

            modelBuilder.Entity<Order>(e => {
                e.HasKey(k => k.Id);
                e.Property(p => p.CloseDescription).HasMaxLength(512).IsRequired();

                e.HasOne(u => u.ApplicationUser).WithMany(r => r.Orders)
                    .HasForeignKey(up => up.ApplicationUserId).OnDelete(DeleteBehavior.ClientNoAction);

                e.HasOne(u => u.Table).WithMany(r => r.Orders)
                    .HasForeignKey(up => up.TableId).OnDelete(DeleteBehavior.ClientNoAction);
            });

            modelBuilder.Entity<DishOrder>(e => {
                e.HasKey(k => new { k.DishId, k.OrderId });

                e.HasOne(u => u.Dish).WithMany(r => r.DishOrders)
                    .HasForeignKey(up => up.DishId).OnDelete(DeleteBehavior.ClientNoAction);

                e.HasOne(u => u.Order).WithMany(r => r.DishOrders)
                    .HasForeignKey(up => up.OrderId).OnDelete(DeleteBehavior.ClientNoAction);
            });

            modelBuilder.Entity<Table>(e => {
                e.HasKey(k => k.Id);
                e.Property(p => p.Name).HasMaxLength(256).IsRequired();
                e.Property(p => p.Description).HasMaxLength(512).IsRequired();
            });

            modelBuilder.Entity<Dish>(e => {
                e.HasKey(k => k.Id);
                e.Property(p => p.Name).HasMaxLength(256).IsRequired();
                e.Property(p => p.Description).HasMaxLength(512).IsRequired();
                e.Property(p => p.Ingredients).HasMaxLength(512).IsRequired();
                e.Property(p => p.Cost).HasPrecision(7,2);

                e.HasOne(u => u.DishCategory).WithMany(r => r.Dishes)
                    .HasForeignKey(up => up.DishCategoryId).OnDelete(DeleteBehavior.ClientNoAction);
            });

            modelBuilder.Entity<DishCategory>(e => {
                e.HasKey(k => k.Id);
                e.Property(p => p.Name).HasMaxLength(256).IsRequired();
                e.Property(p => p.Description).HasMaxLength(512).IsRequired();
            });
        }
    }
}
