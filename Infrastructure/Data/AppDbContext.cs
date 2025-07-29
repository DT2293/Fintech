using Microsoft.EntityFrameworkCore;
using Infrastructure.Entities;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // DbSet cho các bảng
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Function> Functions { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<WalletTransaction> Transactions { get; set; } = null!;
        public DbSet<Currency> Currency { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình khóa chính tổng hợp cho bảng trung gian
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.FunctionId });

            modelBuilder.Entity<Account>()
    .Property(a => a.Balance)
    .HasPrecision(18, 2); // precision: tổng số chữ số, scale: số chữ số sau dấu phẩy

            modelBuilder.Entity<WalletTransaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);


            modelBuilder.Entity<WalletTransaction>()
              .HasOne(t => t.FromAccount)
              .WithMany()
              .HasForeignKey(t => t.FromAccountId)
              .OnDelete(DeleteBehavior.Cascade); // giữ nguyên cascade ở đây

            modelBuilder.Entity<WalletTransaction>()
                .HasOne(t => t.ToAccount)
                .WithMany()
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict); // hoặc .NoAction
        }
    }
}
