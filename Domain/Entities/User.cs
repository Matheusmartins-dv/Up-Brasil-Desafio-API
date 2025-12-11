using Domain.Constants;
using Domain.Exceptions;
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
            throw new FieldRequiredException("Documento");

        if(!Document.IsValidDocument())
            throw new InvalidDocumentException();
   }
   private void ValidateEmail()
   {
         if(string.IsNullOrWhiteSpace(Email))
               throw new FieldRequiredException("Email");
   
         if(!Email.IsValidEmail())
               throw new EmailIsNotValidException();
   }
   private void ValidateName()
   {
        if(string.IsNullOrWhiteSpace(Document))
            throw new FieldRequiredException("Nome");
   }
   private void ValidatePassword()
   {
        if(string.IsNullOrWhiteSpace(Password))
            throw new FieldRequiredException("Senha");
   }
   private void ValidateAll(){
      ValidateDocument();
      ValidateName();
      ValidatePassword();
      ValidateEmail();
   }
   public void UpdatePassword(string newPassword)
   {
      Password = newPassword;

      ValidatePassword();
   }
   public void UpdateName(string newName)
   {
      Name = newName;

      ValidateName();
   }  
   public void UpdateDocument(string newDocument)
   {
      Document = newDocument;

      ValidateDocument();
   }
   public void UpdateEmail(string newEmail)
   {
      Email = newEmail;

      ValidateEmail();
   }

   public class Builder
   {
      private readonly User _user = new();

      public Builder SetEmail(string email)
      {
         _user.Email = email;
         return this;
      }

      public Builder SetPassword(string? password)
      {
         _user.Password = password ?? DefaultValuesDomainConstants.DefaultPassword;
         
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
         _user.ValidateAll();

         return _user;
      }
   }
}