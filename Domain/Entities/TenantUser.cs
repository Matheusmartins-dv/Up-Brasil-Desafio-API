namespace Domain.Entities;

public class TenantUser : EntityBase
{
 public Guid UserId { get; private set; }
 public User? User { get; private set; }
 public Guid TenantId { get; private set; }
 public Tenant? Tenant { get; private set; } 
}
