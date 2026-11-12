using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KodaStore.Api.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Products> Products => Set<Products>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Products>(entity =>
        {
            entity.ToTable("Products", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Product_Price", "[Price] >= 0");
                table.HasCheckConstraint("CK_Product_Stock", "[Stock] >= 0");
            });

            entity.HasKey(p => p.ProductCode);

            entity.Property(p => p.ProductCode)
                .HasMaxLength(30)
                .IsRequired()
                .ValueGeneratedNever();
            
            entity.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(18,2);

            entity.Property(p => p.Stock)
                .IsRequired();
            
            entity.Property(p => p.CategoryId)
                .IsRequired();

            entity.HasOne(p => p.category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasIndex(p => new {p.ProductCode}).IsUnique();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(p => p.CategoryId);
            entity.Property(p => p.CategoryId).ValueGeneratedOnAdd();
            entity.Property(p => p.CategoryName).IsRequired();
        });


        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Orders_Status", "[Status] in ('Pending', 'Confirmed')");
            });

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id).ValueGeneratedOnAdd();

            entity.Property(p => p.OrderNumber).HasMaxLength(35).IsRequired();

            entity.Property(p => p.OrderDate).HasColumnType("datetimeoffset").IsRequired();

            entity.Property(p => p.Status).HasMaxLength(20).IsRequired();

            entity.HasIndex(p => p.OrderDate);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem", "dbo", table =>
            {
                table.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] > 0");
                table.HasCheckConstraint("CK_OrderItem_UnitPrice", "[UnitPrice] >= 0");
            });
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.ProductCode).HasMaxLength(30).IsRequired();
            entity.Property(i => i.Quantity).IsRequired();
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2).IsRequired();
            entity.Property(i => i.SubTotal).HasComputedColumnSql("[Quantity] * [UnitPrice]", stored: true);

            entity.HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Products)
                .WithMany(p => p.items)
                .HasForeignKey(i => i.ProductCode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(i => new { i.Id, i.ProductCode }).IsUnique();
            entity.HasIndex(i => i.ProductCode);
        });
    }
}