using Microsoft.EntityFrameworkCore;
using CoreApiWithEntity.DAL.Models;

namespace CoreApiWithEntity.DAL
{
    public class MyAppDbContext:DbContext
    {
        public MyAppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> Product { get; set; }
        public DbSet<CategoryMst> CategoryMst { get; set; }
        public DbSet<ProductCategoryMst> ProductCategoryMst { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            {
                // Configure Product entity
                modelBuilder.Entity<Product>()
                    .HasKey(p => p.Id);

                modelBuilder.Entity<Product>()
                    .Property(p => p.ProductName)
                    .IsRequired()
                    .HasMaxLength(50);

                modelBuilder.Entity<Product>()
                    .Property(p => p.ProductDetail)
                    .IsRequired()
                    .HasMaxLength(200);

                modelBuilder.Entity<Product>()
                    .Property(p => p.Price)
                    .IsRequired();

                modelBuilder.Entity<Product>()
                    .Property(p => p.Qty)
                    .IsRequired();

                modelBuilder.Entity<Product>()
                    .HasMany(p => p.ProductCategories)
                    .WithOne(pc => pc.Product)
                    .HasForeignKey(pc => pc.ProductId);

                // Configure CategoryMst entity
                modelBuilder.Entity<CategoryMst>()
                    .HasKey(c => c.Id);

                modelBuilder.Entity<CategoryMst>()
                    .Property(c => c.Name)
                    .IsRequired();

                
                // Configure ProductCategoryMst entity
                modelBuilder.Entity<ProductCategoryMst>()
                    .HasKey(pc => new { pc.ProductId, pc.CategoryId });

                modelBuilder.Entity<ProductCategoryMst>()
                    .HasOne(pc => pc.Product)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(pc => pc.ProductId);

                modelBuilder.Entity<ProductCategoryMst>()
                    .HasOne(pc => pc.Category)
                    .WithMany()
                    .HasForeignKey(pc => pc.CategoryId);

                modelBuilder.Entity<UserRole>()
        .HasKey(ur => new { ur.UserId, ur.RoleId });

            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
