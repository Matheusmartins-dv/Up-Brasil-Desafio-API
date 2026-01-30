using Domain.Common.QueryObject;

namespace Domain.Interfaces;

public interface IHelperFilter : IHelperPagination
{
    IQueryable<TEntity> Apply<TEntity, TFields>(IQueryable<TEntity> query, Filter<TFields> filter) 
        where TFields : Enum;
}
