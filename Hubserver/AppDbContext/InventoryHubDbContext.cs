using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace HubServer
{
    public class InventoryHubDbContext : DbContext
    {   
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=MyAppDb;User Id=sa;Password=Thomas@Kafando12;TrustServerCertificate=True;");
        }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Category> Categories{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>( entity =>
            {
                entity.HasOne(e => e.Category)
                     .WithMany(e => e.Products);
                entity.Property(e => e.ProductID)
                      .ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Category>( entity =>
            {
                entity.Property(e => e.CategoryID)
                      .ValueGeneratedOnAdd();
            });

            
        }
        
    }
}