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
      public Builder SetTenant(Tenant tenant)
      {
         _tenantUser.TenantId = tenant.Id; 
         _tenantUser.Tenant = tenant;    

         return this;
      }
      
      public Builder SetUser(User user)
      {
         _tenantUser.UserId = user.Id;
         _tenantUser.User = user;

         return this;
      }

      public TenantUser Build()
      {   
         return _tenantUser;
      }
   }
}
