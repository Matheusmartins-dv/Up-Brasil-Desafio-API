namespace Domain.Entities;

public class Tenant : EntityBase
{
   public ICollection<TenantUser>? TenantUsers { get; private set; }
   public ICollection<Product>? Products { get; private set; }
   public ICollection<ProductCategory>? ProductCategories { get; private set; }
}
