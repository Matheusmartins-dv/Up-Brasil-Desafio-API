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

namespace Application.Features.V1.ProductCategories;

public record ChangeStatusProductCategoryCommand(Guid Id) : IRequest<bool>;

public class ChangeStatusProductCategoryHandler(UpContext context) : IRequestHandler<ChangeStatusProductCategoryCommand, bool>
{
    public async Task<bool> Handle(ChangeStatusProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = await context.ProductCategory.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken) ?? throw new NotFoundException("Categoria de produto");

        productCategory.ChangeStatus();
        
        context.ProductCategory.Update(productCategory);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class ChangeStatusProductCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch($"{RouteConstants.ApiV1}{RouteConstants.Product}{RouteConstants.Category}/{{id}}/status", async (
            [FromRoute] Guid id,
            ISender sender) =>
        {
            var command = new ChangeStatusProductCategoryCommand(id);
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("ChangeStatusProductCategory");
    }
}
