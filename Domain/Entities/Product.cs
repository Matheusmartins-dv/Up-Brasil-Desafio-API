namespace Domain.Entities;

public class Product : EntityBase
{
   public string Name { get; private set; } = string.Empty;
   public string SKU  { get; private set; } = string.Empty;
   public decimal Price { get; private set; } = 0;
   public bool Perishable { get; private set; } = false;
   public Guid TenantId { get; private set; }
   public Tenant? Tenant { get; private set; }
   public Guid CategoryId { get; private set; }
   public ProductCategory? Category { get; private set; }
}
