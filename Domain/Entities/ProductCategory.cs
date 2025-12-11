namespace Domain.Entities;

public class ProductCategory : EntityBase
{
public string Name { get; private set; } = string.Empty;
public Guid TenantId { get; private set; }
public Tenant? Tenant { get; private set; }
public ICollection<Product>? Products { get; private set; }
}
