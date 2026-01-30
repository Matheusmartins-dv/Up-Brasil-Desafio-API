using Application.Common.Behaviors;
using Application.Common.Constants;
using Carter;
using Domain.Common.QueryObject;
using Domain.Enums;
using Domain.Interfaces;
using Infra.Data.Context;
using Infra.Data.Extensions; 
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
    Guid CategoryId,
    string Name,
    string Description,
    string Sku,
    decimal Price,
    bool Perishable);
public record GetProductQuery(Guid TenantId, Filter<ProductsFieldsFilterEnum>? Filter) 
    : IRequest<PagedResult<ProductResponse>>;

public class GetProductCategoriesHandler(UpContext context, IHelperFilter helperFilter) 
    : IRequestHandler<GetProductQuery, PagedResult<ProductResponse>>
{
    public async Task<PagedResult<ProductResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var query = context.Product
            .AsNoTracking()
            .Where(w => w.TenantId == request.TenantId);

        var queryFiltrada = helperFilter.Apply(query, request.Filter ?? new Filter<ProductsFieldsFilterEnum>());

        return await queryFiltrada
            .Select(t => new ProductResponse(
                t.Id,
                t.CreatedAt,
                t.Active,
                t.CategoryId,
                t.Name ?? string.Empty,
                t.Description ?? string.Empty,
                t.SKU ?? string.Empty,
                t.Price,
                t.Perishable))
            .ToPagedResultAsync(helperFilter, request.Filter, cancellationToken);
    }
}

public class GetProductCategoriesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{RouteConstants.ApiV1}{RouteConstants.Tenant}/{{id}}{RouteConstants.Product}", 
            async (ISender sender, [FromRoute] Guid id, [FromQuery] Filter<ProductsFieldsFilterEnum>? filter) =>
        {
            var query = new GetProductQuery(id, filter);
            var result = await sender.Send(query);

            return Results.Ok(new ApiResponse<PagedResult<ProductResponse>>(result));
        })
        .WithName("GetProductsByTenantId")
        .WithTags("Products"); 
    }
}