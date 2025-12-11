using Application.Common.Behaviors;
using Application.Common.Constants;
using Application.Exceptions;
using Carter;
using Domain.Interfaces;
using Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.V1.Products;

public record UpdateProductCommand(
    Guid Id,
    Guid TenantId,
    Guid ProductCategoryId,
    string SKU,
    string Name,
    string Description,
    decimal Price,
    bool Perishable) : IRequest<bool>;

public class UpdateProductHandler(UpContext context, IProductValidationService productService) : IRequestHandler<UpdateProductCommand, bool>
{
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Product.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken) ?? throw new NotFoundException("Produto");

        await productService.ValidateRegisterAndUpdate(request.TenantId, request.ProductCategoryId, request.SKU, cancellationToken);
        
        product.UpdateName(request.Name);
        product.UpdateDescription(request.Description);
        product.UpdatePrice(request.Price);
        product.UpdateSKU(request.SKU);
        product.UpdatePerishable(request.Perishable);
        product.UpdateCategory(request.ProductCategoryId);
        
        context.Product.Update(product);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut($"{RouteConstants.ApiV1}{RouteConstants.Product}", async (
            [FromBody] UpdateProductCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("UpdateProduct");
    }
}
