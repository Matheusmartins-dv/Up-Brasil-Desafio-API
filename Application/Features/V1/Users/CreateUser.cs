using Carter;
using Domain.Entities;
using Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.V1.Users;

public record CreateUserCommand(
    Guid TenantId,
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<bool>;

public class CreateUserHandler(UpContext context) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User.Builder()
            .SetName($"{request.FirstName} {request.LastName}")
            .SetEmail(request.Email)
            .SetPassword(request.Password)
            .Build();
        
        context.User.Add(user);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users", async (
            [FromBody] CreateUserCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("CreateUser");
    }
}
