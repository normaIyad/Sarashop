using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sarashop.Models;

namespace Sarashop.DataBase
{
    public class DatabaseConfigration : IdentityDbContext<ApplecationUser>
    {
        public DatabaseConfigration(DbContextOptions<DatabaseConfigration> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Prodact>(entity =>
            {
                entity.Property(p => p.Name)
                    .HasColumnType("varchar").HasMaxLength(50);
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Price)
                    .HasColumnType("decimal(6,2)");
                entity.Property(p => p.Description)
                .HasPrecision(10, 2);
            });

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Prodacts)
                .WithOne(e => e.Category)
                .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<Brand>().HasMany(e => e.Prodacts)
                .WithOne(e => e.Brand).HasForeignKey(p => p.BrandID);
            modelBuilder.Entity<Cart>()
      .HasKey(c => new { c.ProductId, c.ApplecationUserId });

        }
        public DbSet<Prodact> Prodacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set; }
        public DbSet<PasswordResetCode> passwordResetCodes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewImgs> ReviewsImgs { get; set; }
    }
}
