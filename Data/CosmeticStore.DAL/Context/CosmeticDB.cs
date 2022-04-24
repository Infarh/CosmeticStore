using CosmeticStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.DAL.Context;

public class CosmeticDB : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;

    public CosmeticDB(DbContextOptions<CosmeticDB> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        model.Entity<Product>()
           .Property(product => product.Price)
           .HasPrecision(18, 2);

        model.Entity<Customer>()
           .HasIndex(p => p.Name)
           .IsUnique();
    }
}
