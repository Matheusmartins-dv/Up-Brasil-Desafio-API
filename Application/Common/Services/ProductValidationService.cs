using Application.Exceptions;
using Domain.Interfaces;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Services;

public class ProductValidationService(UpContext context) : IProductValidationService
{
    public async Task ValidateRegisterAndUpdate(Guid tenantId, Guid categoryId, string sku, CancellationToken cancellationToken)
    {
        var tenantExists = await context.Tenant.Include(i => i.ProductCategories)
                                               .Include(i => i.Products)
                                               .FirstOrDefaultAsync(a => a.Id == tenantId, cancellationToken) ?? throw new NotFoundException("Tenant");

        var categoryExist = tenantExists.ProductCategories?.FirstOrDefault(f => f.Id == categoryId) ?? throw new NotFoundException("Categoria de produto");

        if (!categoryExist.Active)
            throw new ProductCategoryDeactivedException();

        var productWithSameSKU = tenantExists.Products?.FirstOrDefault(f => f.SKU == sku);

        if (productWithSameSKU != null)
            throw new DuplicateProductSKUException(sku);
    }
}
