using System;

namespace Domain.Common.QueryObject;

public class Pagination
{
   public int PageSize { get; set; } = 10;
   public int Page { get; set; } = 1;
}
