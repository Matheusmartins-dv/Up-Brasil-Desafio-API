using System.Linq.Expressions;
using System.Reflection;
using Domain.Common.QueryObject;
using Domain.Enums;
using Domain.Interfaces;

namespace Infra.Data.Helpers;

public class HelperFilter : HelperPagination, IHelperFilter
{
    public IQueryable<TEntity> Apply<TEntity, TField>(IQueryable<TEntity> query, Filter<TField> filter)
        where TField : Enum
    {
        if (filter == null || filter.Items == null || !filter.Items.Any())
            return query;

        var resultQuery = query;

        foreach (var item in filter.Items)
        {
            var valueIsEmpty = item.Value == null ||
                               (item.Value is string str && string.IsNullOrWhiteSpace(str));

            if (item.IsRequired && valueIsEmpty)
                throw new ArgumentException($"O campo {item.Field} é obrigatório e não foi preenchido.");

            if (valueIsEmpty) continue;

            string fieldName = item.Field.ToString();
            PropertyInfo? prop = typeof(TEntity).GetProperty(fieldName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (prop == null) continue;

            var parameter = Expression.Parameter(typeof(TEntity), "x");

            var comparison = GetComparisonExpression(parameter, prop, item.Operator, item.Value);

            var lambda = Expression.Lambda<Func<TEntity, bool>>(comparison, parameter);
            resultQuery = resultQuery.Where(lambda);
        }

        return resultQuery;
    }

    private Expression GetComparisonExpression(
    ParameterExpression parameter,
    PropertyInfo prop,
    OperationFilterEnum op,
    object value)
    {
        var propertyAccess = Expression.Property(parameter, prop);
        var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

        var rawValue = value is System.Text.Json.JsonElement jsonElement
        ? jsonElement.ToString()
        : value?.ToString() ?? string.Empty;

        object convertedValue = rawValue;

        if (targetType == typeof(Guid))
            convertedValue = Guid.Parse(rawValue);

        if (targetType.IsEnum)
            convertedValue = Enum.Parse(targetType, rawValue);

        if (convertedValue is string && targetType != typeof(string))
            convertedValue = Convert.ChangeType(rawValue, targetType);

        var constant = Expression.Constant(convertedValue, prop.PropertyType);

        return op switch
        {
            OperationFilterEnum.Equals => Expression.Equal(propertyAccess, constant),
            OperationFilterEnum.NotEquals => Expression.NotEqual(propertyAccess, constant),
            OperationFilterEnum.GreaterThan => Expression.GreaterThan(propertyAccess, constant),
            OperationFilterEnum.LessThan => Expression.LessThan(propertyAccess, constant),
            OperationFilterEnum.Contains when prop.PropertyType == typeof(string) =>
                Expression.Call(propertyAccess, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, constant),
            _ => Expression.Equal(propertyAccess, constant)
        };
    }
}
