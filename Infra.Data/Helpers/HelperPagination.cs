using System;
using Domain.Common.QueryObject;
using Domain.Interfaces;

namespace Infra.Data.Helpers;

public class HelperPagination :  IHelperPagination
{
    public IQueryable<T> ApplyPagination<T>(IQueryable<T> query, Pagination pagination)
    {
        int pageSize = pagination.PageSize > 0 ? pagination.PageSize : 10;
        int page = pagination.Page > 0 ? pagination.Page : 1;

        int skip = (page - 1) * pageSize;

        return query.Skip(skip).Take(pageSize);
    }
}
