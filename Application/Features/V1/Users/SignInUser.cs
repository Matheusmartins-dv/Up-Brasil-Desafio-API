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

namespace Application.Features.V1.Users;
public record SignInUserResponse(
    Guid Id,
    ICollection<Guid> TenantIds
);
public record SignInUserCommand(
    string Email,
    string Password) : IRequest<SignInUserResponse>;

public class SignInUserHandler(UpContext context) : IRequestHandler<SignInUserCommand, SignInUserResponse>
{
    public async Task<SignInUserResponse> Handle(SignInUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.User
            .Include(i => i.TenantUsers)!
            .ThenInclude(t => t.Tenant)
            .FirstOrDefaultAsync(f => f.Email == request.Email && f.Password == request.Password, cancellationToken) 
            ?? throw new NotFoundException("Usuário");
        
        var tenantUsers = user.TenantUsers!.Where(w => w.Active).Select(s => s.TenantId).ToList();

        if(!tenantUsers.Any())
            throw new UnauthorizedAccessException("Usuário não está associado a nenhum tenant");

        return new SignInUserResponse(
            user.Id,
            tenantUsers
        );
        
    }
}

public class SignInUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{RouteConstants.ApiV1}{RouteConstants.User}{RouteConstants.SignIn}", async (
            [FromBody] SignInUserCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<SignInUserResponse>(result));
        })
        .WithName("SignInUser");
    }
}
