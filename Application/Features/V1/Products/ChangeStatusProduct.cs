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

public record ChangeStatusProductCommand(Guid Id) : IRequest<bool>;

public class ChangeStatusProductHandler(UpContext context) : IRequestHandler<ChangeStatusProductCommand, bool>
{
    public async Task<bool> Handle(ChangeStatusProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Product.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken) ?? throw new NotFoundException("Produto");

        product.ChangeStatus();
        
        context.Product.Update(product);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class ChangeStatusProducEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch($"{RouteConstants.ApiV1}{RouteConstants.Product}", async (
            [FromBody] ChangeStatusProductCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("ProductChangeStatus");
    }
}
