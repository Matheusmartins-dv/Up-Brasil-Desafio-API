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

namespace Application.Features.V1.TenantUsers;

public record TenantUserResponse(
    Guid Id,
    string UserName,
    string UserEmail,
    string UserDocument,
    string TenantDescription,
    bool Active,
    DateTime CreatedAt);

public record GetTenantUserQuery(Guid TenantId) : IRequest<ICollection<TenantUserResponse>>;

public class GetTenantsHandler(UpContext context) : IRequestHandler<GetTenantUserQuery, ICollection<TenantUserResponse>>
{
    public async Task<ICollection<TenantUserResponse>> Handle(GetTenantUserQuery request, CancellationToken cancellationToken)
    {
        var tenantUsers = await context.TenantUser
            .AsNoTracking()
            .Include(i => i.User)
            .Include(i => i.Tenant)
            .Where(w => w.TenantId == request.TenantId)
            .Select(t => new TenantUserResponse(
                t.Id,
                t.User!.Name ?? string.Empty,
                t.User.Email ?? string.Empty,
                t.User.Document ?? string.Empty,
                t.Tenant!.Description ?? string.Empty,
                t.Active,
                t.CreatedAt))
            .ToListAsync(cancellationToken) ?? throw new NotFoundException("Usuários associados");

        return tenantUsers;
    }
}

public class GetTenantUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{RouteConstants.ApiV1}{RouteConstants.Tenant}/{{id}}{RouteConstants.User}", async (ISender sender, [FromRoute] Guid id) =>
        {
            var query = new GetTenantUserQuery(id);
            var result = await sender.Send(query);
            
            return Results.Ok(new ApiResponse<ICollection<TenantUserResponse>>(result));
        })
        .WithName("GetTenantUsers");
    }
}
