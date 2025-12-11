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
    bool Perishable) : IRequest<bool>;

public class CreateProductHandler(UpContext context) : IRequestHandler<CreateProductCommand, bool>
{
    public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var tenantExists = await context.Tenant.FirstOrDefaultAsync(a => a.Id == request.TenantId, cancellationToken) ?? throw new NotFoundException("Tenant");

        var categoryExist = await context.ProductCategory.FirstOrDefaultAsync(a => a.Id == request.ProductCategoryId, cancellationToken) ?? throw new NotFoundException("Categoria de produto");

        var product = new Product.Builder()
            .SetSKU(request.SKU)
            .SetPerishable(request.Perishable)
            .SetName(request.Name)
            .SetDescription(request.Description)
            .SetTenantId(request.TenantId)
            .SetCategoryId(request.ProductCategoryId)
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
