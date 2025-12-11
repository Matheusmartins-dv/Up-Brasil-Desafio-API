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

public record UpdateProductCategoryCommand(
    Guid Id,
    string Name,
    string Description) : IRequest<bool>;

public class UpdateProductCategoryHandler(UpContext context) : IRequestHandler<UpdateProductCategoryCommand, bool>
{
    public async Task<bool> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = await context.ProductCategory.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken) ?? throw new NotFoundException("Categoria de produto");

        productCategory.UpdateName(request.Name);
        productCategory.UpdateDescription(request.Description);
        
        context.ProductCategory.Update(productCategory);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class UpdateProductCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut($"{RouteConstants.ApiV1}{RouteConstants.Product}{RouteConstants.Category}", async (
            [FromBody] UpdateProductCategoryCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("UpdateProductCategory");
    }
}
