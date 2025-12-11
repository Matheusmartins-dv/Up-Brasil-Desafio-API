using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configuration;

public class ProductCategoryConfiguration : EntityBaseConfiguration<ProductCategory>, IEntityTypeConfiguration<ProductCategory>
{
    public override void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        base.Configure(builder);

        builder.HasOne(l => l.Tenant).WithMany(w => w.ProductCategories).HasForeignKey(h => h.TenantId);
    }
}
