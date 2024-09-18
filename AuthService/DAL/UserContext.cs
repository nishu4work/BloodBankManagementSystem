using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DAL
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Donor relationships
            modelBuilder.Entity<User>()
                .HasOne(d => d.Role)
                .WithMany(bt => bt.Users)
                .HasForeignKey(d => d.RoleId);

            // Configure Role relationship
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Hospital" },
                new Role { RoleId = 3, RoleName = "Donor" }
            );

            // You can seed users for testing
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Password = "admin123", Email = "admin@example.com", RoleId = 1 },
                new User { UserId = 2, Password = "hospital123", Email = "hospital@example.com", RoleId = 2 },
                new User { UserId = 3, Password = "donor123", Email = "donor@example.com", RoleId = 3 }
            );
        }
    }
}
