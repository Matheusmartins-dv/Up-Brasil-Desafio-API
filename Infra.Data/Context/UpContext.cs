using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context;

public class UpContext(DbContextOptions<UpContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<Tenant> Tenant { get; set; }
    public DbSet<TenantUser> TenantUser { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }
    public DbSet<Product> Product{ get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UpContext).Assembly);
    }
}
