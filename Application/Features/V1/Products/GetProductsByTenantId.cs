using Application.Common.Behaviors;
using Application.Common.Constants;
using Application.Exceptions;
using Carter;
using Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.V1.Products;

public record ProductResponse(
    Guid Id,
    DateTime CreatedAt,
    bool Active,
    string Name,
    string Description,
    string SKU,
    decimal Price,
    bool Perishable);

public record GetProductQuery(Guid TenantId) : IRequest<ICollection<ProductResponse>>;

public class GetProductCategoriesHandler(UpContext context) : IRequestHandler<GetProductQuery, ICollection<ProductResponse>>
{
    public async Task<ICollection<ProductResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var products = await context.Product
            .AsNoTracking()
            .Where(w => w.TenantId == request.TenantId)
            .Select(t => new ProductResponse(
                t.Id,
                t.CreatedAt,
                t.Active,
                t.Name ?? string.Empty,
                t.Description ?? string.Empty,
                t.SKU ?? string.Empty,
                t.Price,
                t.Perishable))
            .ToListAsync(cancellationToken) ?? throw new NotFoundException("Produtos");

        return products;
    }
}

public class GetProductCategoriesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{RouteConstants.ApiV1}{RouteConstants.Tenant}/{{id}}{RouteConstants.Product}", async (ISender sender, [FromRoute] Guid id) =>
        {
            var query = new GetProductQuery(id);
            var result = await sender.Send(query);
            
            return Results.Ok(new ApiResponse<ICollection<ProductResponse>>(result));
        })
        .WithName("GetProductsByTenantId");
    }
}
