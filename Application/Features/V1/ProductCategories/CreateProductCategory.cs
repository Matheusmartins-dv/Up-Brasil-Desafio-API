using Application.Common.Behaviors;
using Application.Common.Constants;
using Application.Exceptions;
using Carter;
using Domain.Entities;
using Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.V1.ProductCategories;

public record CreateProductCategoryCommand(
    string Name,
    string Description,
    Guid TenantId) : IRequest<bool>;

public class CreateProductCategoryHandler(UpContext context) : IRequestHandler<CreateProductCategoryCommand, bool>
{
    public async Task<bool> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var tenantExists = await context.Tenant.FirstOrDefaultAsync(a => a.Id == request.TenantId, cancellationToken) ?? throw new NotFoundException("Tenant");

        var productCategory = new ProductCategory.Builder()
            .SetName(request.Name)
            .SetDescription(request.Description)
            .SetTenantId(request.TenantId)
            .Build();
        
        context.ProductCategory.Add(productCategory);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}

public class CreateProductCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{RouteConstants.ApiV1}{RouteConstants.Product}{RouteConstants.Category}", async (
            [FromBody] CreateProductCategoryCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            
            return Results.Ok(new ApiResponse<bool>(result));
        })
        .WithName("CreateProductCategory");
    }
}
