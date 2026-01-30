using System;
using Domain.Common.QueryObject;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        IHelperPagination paginationHelper,
        Pagination? pagination,
        CancellationToken cancellationToken = default)
    {
        var validPagination = pagination ?? new Pagination();

        var totalItems = await query.CountAsync(cancellationToken);

        var paginatedQuery = paginationHelper.ApplyPagination(query, validPagination);

        var items = await paginatedQuery.ToListAsync(cancellationToken);

        return new PagedResult<T>(
            items, 
            totalItems, 
            validPagination.Page, 
            validPagination.PageSize);
    }
}
