using Domain.Entities;

namespace Domain.Interfaces;

public interface IProductValidationService
{
   Task ValidateRegisterAndUpdate(Product product, Guid categoryId, string sku, CancellationToken cancellationToken);
}
