namespace Domain.Entities;

public class TenantUser : EntityBase
{
   public Guid UserId { get; private set; }
   public User? User { get; private set; }
   public Guid TenantId { get; private set; }
   public Tenant? Tenant { get; private set; } 
   public class Builder
   {
      private readonly TenantUser _tenantUser = new();
      public Builder SetUser(Guid useId)
      {
         _tenantUser.UserId = useId;

         return this;
      }
     public Builder SetTenant(Guid tenantId)
      {
         _tenantUser.TenantId = tenantId;
         
         return this;
      }

      public TenantUser Build()
      {   
         return _tenantUser;
      }
   }
}
