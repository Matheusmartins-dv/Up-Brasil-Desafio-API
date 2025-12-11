namespace Domain.Interfaces;

public interface IProductValidationService
{
   Task ValidateRegisterAndUpdate(Guid tenantId, Guid categoryId, string sku, CancellationToken cancellationToken);
}
