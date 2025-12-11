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

namespace Application.Features.V1.Users;

public record UpdateUserCommand(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string Document) : IRequest<bool>;

public class UpdateUserHandler(UpContext context, IUserValidationService userService) : IRequestHandler<UpdateUserCommand, bool>
{
    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.User.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken) ?? throw new NotFoundException("Usuário");

        if (user.Email != request.Email)
             await userService.ValidateUniquenessEmail(request.Email, cancellationToken);

        if(user.Document !=  request.Document)
             await userService.ValidateUniquenessDocument(request.Document, cancellationToken);

        user.UpdateName($"{request.FirstName} {request.LastName}");
        user.UpdateEmail(request.Email);
        user.UpdateDocument(request.Document);
        
        context.User.Update(user);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut($"{RouteConstants.ApiV1}{RouteConstants.User}", async (
            [FromBody] UpdateUserCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("UpdateUser");
    }
}
