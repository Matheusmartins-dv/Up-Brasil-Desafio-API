using Application.Common.Behaviors;
using Application.Common.Constants;
using Carter;
using Domain.Entities;
using Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.V1.TenantUsers;

public record AssociateUserToTenantCommand(
    Guid UserId,
    Guid TenantId) : IRequest<bool>;

public class AssociateUserToTenantHandler(UpContext context) : IRequestHandler<AssociateUserToTenantCommand, bool>
{
    public async Task<bool> Handle(AssociateUserToTenantCommand request, CancellationToken cancellationToken)
    {
        var user = await context.User.FirstOrDefaultAsync(f => f.Id == request.UserId, cancellationToken) ?? throw new Exception("Usuário não encontrado.");

        var tenant = await context.Tenant.FirstOrDefaultAsync(f => f.Id == request.TenantId, cancellationToken) ?? throw new Exception("Tenant não encontrado.");

        var tenatUser = new TenantUser.Builder()
            .SetTenant(tenant)
            .SetUser(user)
            .Build();
        
        context.TenantUser.Add(tenatUser);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class AssociateUserToTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{RouteConstants.ApiV1}{RouteConstants.Tenant}{RouteConstants.User}", async (
            [FromBody] AssociateUserToTenantCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("AssociateUserToTenant");
    }
}
