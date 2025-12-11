using Domain.Exceptions;

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
   private void Validate()
   {
        if(string.IsNullOrWhiteSpace(Description))
            throw new FieldRequiredException("Descrição");
        
        if(string.IsNullOrWhiteSpace(Name))
            throw new FieldRequiredException("Nome da categoria");
         
        if(Price < 0)
            throw new ProductValueNotBeNegativeException();
         
        if(string.IsNullOrWhiteSpace(SKU))
            throw new FieldRequiredException("SKU");
   }

   public class Builder
   {
      private readonly Product _product = new();
      public Builder SetDescription(string description)
      {
         _product.Description = description;
         
         return this;
      }
      public Builder SetName(string name)
      {
         _product.Name = name?.Trim() ?? string.Empty;

         return this;
      }
      public Builder SetSKU(string sku)
      {
         _product.SKU = sku?.ToUpper() ?? string.Empty;

         return this;
      }
      public Builder SetPrice(decimal price)
      {
         _product.Price = price;

         return this;
      }
      public Builder SetPerishable(bool perishable)
      {
         _product.Perishable = perishable;

         return this;
      }
      public Builder SetTenantId(Guid tenantId)
      {
         _product.TenantId = tenantId;

         return this;
      }

      public Product Build()
      {
         _product.Validate();
         
         return _product;
      }
   }
}
