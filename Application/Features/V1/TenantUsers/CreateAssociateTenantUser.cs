using Application.Common.Behaviors;
using Application.Common.Constants;
using Application.Exceptions;
using Carter;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.V1.TenantUsers;

public record AssociateUserToTenantCommand(
    string FirstName,
    string LastName,
    string Document,
    string Email,
    string Password,
    Guid TenantId) : IRequest<bool>;

public class AssociateUserToTenantHandler(UpContext context, IUserValidationService userService) : IRequestHandler<AssociateUserToTenantCommand, bool>
{
    public async Task<bool> Handle(AssociateUserToTenantCommand request, CancellationToken cancellationToken)
    {
        await userService.ValidateUniqueness(request.Email, request.Document, cancellationToken);

        var user = new User.Builder()
            .SetName($"{request.FirstName} {request.LastName}")
            .SetEmail(request.Email)
            .SetPassword(request.Password)
            .SetDocument(request.Document)
            .Build();

        var tenant = await context.Tenant.FirstOrDefaultAsync(f => f.Id == request.TenantId, cancellationToken) ?? throw new NotFoundException("Tenant");

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
