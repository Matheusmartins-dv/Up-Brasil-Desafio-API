using Domain.Extensions;

namespace Domain.Entities;

public class User : EntityBase
{
   public string Email { get; private set; } = string.Empty;
   public string Password { get; private set; } = string.Empty;
   public string Name { get; private set; } = string.Empty;
   public string Document { get; private set; } = string.Empty;
   public ICollection<TenantUser>? TenantUsers { get; private set; }
   private void ValidateDocument()
   {
        if(string.IsNullOrWhiteSpace(Document))
            throw new ArgumentException("Document is required");
            
        if(Document.IsValidDocument() is false)
            throw new ArgumentException("Invalid document");
   }
   public class Builder
   {
      private readonly User _user = new();

      public Builder SetEmail(string email)
      {
         _user.Email = email;
         return this;
      }

      public Builder SetPassword(string password)
      {
         _user.Password = password;
         return this;
      }
      public Builder SetName(string name)
      {
         _user.Name = name;
         return this;
      }
      public Builder SetDocument(string document)
      {
         _user.Document = document;
         return this;
      }

      public User Build()
      {
         _user.ValidateDocument();

         return _user;
      }
   }
}