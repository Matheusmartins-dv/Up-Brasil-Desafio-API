using Domain.Common.QueryObject;

namespace Domain.Interfaces;

public interface IHelperPagination
{
  IQueryable<T> ApplyPagination<T>(IQueryable<T> query, Pagination pagination);
}
