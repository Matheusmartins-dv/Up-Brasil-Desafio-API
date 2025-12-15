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

namespace Application.Features.V1.Products;

public record DeleteProductCommand(
    Guid Id) : IRequest<bool>;

public class DeleteProductHandler(UpContext context) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Product.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken) ?? throw new NotFoundException("Produto");

        context.Product.Remove(product);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{RouteConstants.ApiV1}{RouteConstants.Product}/{{id}}", async (
            [FromRoute] Guid id,
            ISender sender) =>
        {
            var command = new DeleteProductCommand(id);

            var result = await sender.Send(command);

            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("DeleteProduct");
    }
}

