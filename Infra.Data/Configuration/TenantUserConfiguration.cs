using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configuration;

public class TenantUserConfiguration : EntityBaseConfiguration<TenantUser>, IEntityTypeConfiguration<TenantUser>
{
    public override void Configure(EntityTypeBuilder<TenantUser> builder)
    {
        base.Configure(builder);

        builder.HasOne(l => l.Tenant).WithMany(w => w.TenantUsers).HasForeignKey(h => h.TenantId);

        builder.HasOne(l => l.User).WithMany(w => w.TenantUsers).HasForeignKey(h => h.UserId);
    }
}
