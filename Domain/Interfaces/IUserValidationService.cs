namespace Domain.Interfaces;

public interface IUserValidationService
{
   Task ValidateUniqueness(string email, string document, CancellationToken cancellationToken);
}
