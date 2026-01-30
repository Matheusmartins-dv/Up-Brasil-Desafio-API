using Application.Common.Behaviors;
using Application.Common.Constants;
using Carter;
using Domain.Builders;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.V1.Users;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Document,
    string Email,
    string Password) : IRequest<bool>;

public class CreateUserHandler(UpContext context, IUserValidationService userService) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await userService.ValidateUniqueness(request.Email, request.Document, cancellationToken);

        var user = new UserBuilder()
            .SetName($"{request.FirstName} {request.LastName}")
            .SetDocument(request.Document)
            .SetEmail(request.Email)
            .SetPassword(request.Password)
            .Build();

        var tenant = new Tenant.Builder()
            .SetDescription($"Tenant padrão de {request.FirstName} {request.LastName}")
            .Build();
        
        var tenatUser = new TenantUser.Builder()
            .SetTenant(tenant)
            .SetUser(user)
            .Build();

        
        context.User.Add(user);
        context.Tenant.Add(tenant);
        context.TenantUser.Add(tenatUser);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{RouteConstants.ApiV1}{RouteConstants.User}", async (
            [FromBody] CreateUserCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("CreateUser");
    }
}
