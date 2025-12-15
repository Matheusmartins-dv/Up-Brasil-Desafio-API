using Application.Common.Behaviors;
using Application.Common.Constants;
using Carter;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.V1.Products;

public record CreateProductCommand(
    Guid TenantId,
    Guid ProductCategoryId,
    string Sku,
    string Name,
    string? Description,
    decimal Price,
    bool Perishable) : IRequest<bool>;

public class CreateProductHandler(UpContext context, IProductValidationService productService) : IRequestHandler<CreateProductCommand, bool>
{
    public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product.Builder()
            .SetSKU(request.Sku)
            .SetPerishable(request.Perishable)
            .SetName(request.Name)
            .SetDescription(request.Description)
            .SetTenantId(request.TenantId)
            .SetCategoryId(request.ProductCategoryId)
            .SetPrice(request.Price)
            .Build();
        
        await productService.ValidateRegisterAndUpdate(product, request.ProductCategoryId, request.Sku, cancellationToken);

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
