using Domain.Exceptions;

namespace Domain.Entities;

public class ProductCategory : EntityBase
{
   public string Name { get; private set; } = string.Empty;
   public Guid TenantId { get; private set; }
   public Tenant? Tenant { get; private set; }
   public ICollection<Product>? Products { get; private set; }
   private void Validate()
   {
        if(string.IsNullOrWhiteSpace(Description))
            throw new FieldRequiredException("Descrição");
        
        if(string.IsNullOrWhiteSpace(Name))
            throw new FieldRequiredException("Nome da categoria");
   }

   public class Builder
   {
      private readonly ProductCategory _productCategory = new();
      public Builder SetDescription(string description)
      {
         _productCategory.Description = description;

         return this;
      }
      public Builder SetName(string name)
      {
         _productCategory.Name = name;

         return this;
      }

      public ProductCategory Build()
      {
         _productCategory.Validate();
         
         return _productCategory;
      }
   }
}
