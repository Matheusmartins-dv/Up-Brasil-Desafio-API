using Application.Common.Behaviors;
using Application.Common.Constants;
using Carter;
using Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.V1.Tenants;

public record TenantResponse(
    Guid Id,
    string Description,
    bool Active,
    DateTime CreatedAt);

public record GetTenantsQuery() : IRequest<ICollection<TenantResponse>>;

public class GetTenantsHandler(UpContext context) : IRequestHandler<GetTenantsQuery, ICollection<TenantResponse>>
{
    public async Task<ICollection<TenantResponse>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var tenants = await context.Tenant
            .AsNoTracking() 
            .Select(t => new TenantResponse(
                t.Id,
                t.Description ?? string.Empty,
                t.Active,
                t.CreatedAt))
            .ToListAsync(cancellationToken);

        return tenants;
    }
}

public class GetTenantsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{RouteConstants.ApiV1}{RouteConstants.Tenant}", async (ISender sender) =>
        {
            var query = new GetTenantsQuery();
            var result = await sender.Send(query);
            
            return Results.Ok(new ApiResponse<ICollection<TenantResponse>>(result));
        })
        .WithName("GetTenants");
    }
}
