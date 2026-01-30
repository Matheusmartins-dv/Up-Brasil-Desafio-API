using Domain.Constants;
using Domain.Exceptions;
using Domain.Extensions;

namespace Domain.Entities;

public class User : EntityBase
{
   public string Email { get; internal set; } = string.Empty;
   public string Password { get; internal set; } = string.Empty;
   public string Name { get; internal set; } = string.Empty;
   public string Document { get; internal set; } = string.Empty;
   public ICollection<TenantUser>? TenantUsers { get; internal set; }
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
   internal void ValidateAll(){
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
}