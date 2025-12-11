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

namespace Application.Features.V1.ProductCategories;

public record ProductCategoryResponse(
    Guid Id,
    DateTime CreatedAt,
    bool Active,
    string Name,
    string Description);

public record GetProductCategoriesQuery(Guid TenantId) : IRequest<ICollection<ProductCategoryResponse>>;

public class GetProductCategoriesHandler(UpContext context) : IRequestHandler<GetProductCategoriesQuery, ICollection<ProductCategoryResponse>>
{
    public async Task<ICollection<ProductCategoryResponse>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await context.ProductCategory
            .AsNoTracking()
            .Where(w => w.TenantId == request.TenantId)
            .Select(t => new ProductCategoryResponse(
                t.Id,
                t.CreatedAt,
                t.Active,
                t.Name ?? string.Empty,
                t.Description ?? string.Empty))
            .ToListAsync(cancellationToken) ?? throw new NotFoundException("Categorias de produtos");

        return categories;
    }
}

public class GetProductCategoriesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{RouteConstants.ApiV1}{RouteConstants.Tenant}/{{id}}{RouteConstants.Product}{RouteConstants.Category}", async (ISender sender, [FromRoute] Guid id) =>
        {
            var query = new GetProductCategoriesQuery(id);
            var result = await sender.Send(query);
            
            return Results.Ok(new ApiResponse<ICollection<ProductCategoryResponse>>(result));
        })
        .WithName("GetProductCategoriesByTenantId");
    }
}
