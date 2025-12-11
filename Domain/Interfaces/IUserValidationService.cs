namespace Domain.Interfaces;

public interface IUserValidationService
{
   Task ValidateUniqueness(string email, string document, CancellationToken cancellationToken);
   Task ValidateUniquenessEmail(string email, CancellationToken cancellationToken);
   Task ValidateUniquenessDocument(string document, CancellationToken cancellationToken);
}
