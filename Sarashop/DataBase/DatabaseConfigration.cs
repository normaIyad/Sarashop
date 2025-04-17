using Microsoft.EntityFrameworkCore;
using Sarashop.Models;

namespace Sarashop.DataBase
{
    public class DatabaseConfigration : DbContext
    {
        public DatabaseConfigration(DbContextOptions<DatabaseConfigration> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prodact>(entity =>
            {
                entity.Property(p => p.Name)
                    .HasColumnType("varchar").HasMaxLength(50);
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Price)
                    .HasColumnType("decimal(6,2)");
            });

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Prodacts)
                .WithOne(e => e.Category)
                .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<Brand>().HasMany(e => e.Prodacts)
                .WithOne(e => e.Brand).HasForeignKey(p => p.BrandID);
        }
        public DbSet<Prodact> Prodacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
    }
}
