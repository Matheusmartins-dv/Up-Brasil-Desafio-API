using Domain.Exceptions;

namespace Domain.Entities;

public class Tenant : EntityBase
{
   public ICollection<TenantUser>? TenantUsers { get; private set; }
   public ICollection<Product>? Products { get; private set; }
   public ICollection<ProductCategory>? ProductCategories { get; private set; }
   private void Validate()
   {
        if(string.IsNullOrWhiteSpace(Description))
            throw new FieldRequiredException("Descrição");
   }
   
   public class Builder
   {
      private readonly Tenant _tenant = new();
      public Builder SetDescription(string description)
      {
         _tenant.Description = description;
         return this;
      }

      public Tenant Build()
      {
         _tenant.Validate();

         return _tenant;
      }
   }
}
