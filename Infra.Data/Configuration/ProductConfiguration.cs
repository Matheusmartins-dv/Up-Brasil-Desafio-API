using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configuration;

public class ProductConfiguration : EntityBaseConfiguration<Product>, IEntityTypeConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.HasOne(l => l.Tenant).WithMany(w => w.Products).HasForeignKey(h => h.TenantId);
        
        builder.HasOne(l => l.Category).WithMany(w => w.Products).HasForeignKey(h => h.CategoryId);
    }
}
