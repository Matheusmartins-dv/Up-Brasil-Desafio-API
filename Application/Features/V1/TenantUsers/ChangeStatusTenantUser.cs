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

namespace Application.Features.V1.TenantUsers;

public record ChangeStatusTenantUserCommand(
    Guid Id) : IRequest<bool>;

public class ChangeStatusTenantUserHandler(UpContext context) : IRequestHandler<ChangeStatusTenantUserCommand, bool>
{
    public async Task<bool> Handle(ChangeStatusTenantUserCommand request, CancellationToken cancellationToken)
    {
        var tenantUser = await context.TenantUser.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken) ?? throw new NotFoundException("Usuário");

        tenantUser.ChangeStatus();
        
        context.TenantUser.Update(tenantUser);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch($"{RouteConstants.ApiV1}{RouteConstants.Tenant}{RouteConstants.User}", async (
            [FromBody] ChangeStatusTenantUserCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("TenantUserChangeStatus");
    }
}
