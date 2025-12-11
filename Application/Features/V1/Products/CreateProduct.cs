using Application.Common.Behaviors;
using Application.Common.Constants;
using Application.Exceptions;
using Carter;
using Domain.Entities;
using Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.V1.Products;

public record CreateProductCommand(
    Guid TenantId,
    Guid ProductCategoryId,
    string SKU,
    string Name,
    string Description,
    decimal Price,
    bool Perishable) : IRequest<bool>;

public class CreateProductHandler(UpContext context) : IRequestHandler<CreateProductCommand, bool>
{
    public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var tenantExists = await context.Tenant.FirstOrDefaultAsync(a => a.Id == request.TenantId, cancellationToken) ?? throw new NotFoundException("Tenant");

        var categoryExist = await context.ProductCategory.FirstOrDefaultAsync(a => a.Id == request.ProductCategoryId, cancellationToken) ?? throw new NotFoundException("Categoria de produto");

        if(!categoryExist.Active)
            throw new ProductCategoryDeactivedException();   

        var productWithSameSKU = await context.Product.FirstOrDefaultAsync(a => a.SKU == request.SKU && a.TenantId == request.TenantId, cancellationToken);
        
        if(productWithSameSKU != null)
            throw new DuplicateProductSKUException(request.SKU);

        var product = new Product.Builder()
            .SetSKU(request.SKU)
            .SetPerishable(request.Perishable)
            .SetName(request.Name)
            .SetDescription(request.Description)
            .SetTenantId(request.TenantId)
            .SetCategoryId(request.ProductCategoryId)
            .SetPrice(request.Price)
            .Build();
        
        context.Product.Add(product);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{RouteConstants.ApiV1}{RouteConstants.Product}", async (
            [FromBody] CreateProductCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("CreateProduct");
    }
}
