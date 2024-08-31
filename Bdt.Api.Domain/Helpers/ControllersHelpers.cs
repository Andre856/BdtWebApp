using Bdt.Shared.Models.App;
using System.Linq.Expressions;

namespace Bdt.Api.Domain.Helpers;

public static class ControllersHelpers<TEntity>
{
    public static Expression<Func<TEntity, bool>>[]? GetLamdas(List<FilterModel> filters)
    {
        var conditions = new Expression<Func<TEntity, bool>>[filters.Count];

        int idx = 0;
        foreach (var filter in filters)
        {
            var column = filter.Property.Replace(".", "");
            var parameterExpression = Expression.Parameter(typeof(TEntity), "x");
            var propertyExpression = Expression.Property(parameterExpression, column);
            ConstantExpression filterValue;

            switch (filter.ValueType)
            {
                case "int":
                    int.TryParse(filter.FilterValue.ToString(), out int intValue);
                    filterValue = Expression.Constant(intValue);
                    break;
                default:
                    continue;
            }

            BinaryExpression filterExpression;

            switch (filter.FilterOperator)
            {
                case (int)BdtFilterOperator.Equals:
                    filterExpression = Expression.Equal(propertyExpression, filterValue);
                    break;
                case (int)BdtFilterOperator.NotEquals:
                    filterExpression = Expression.NotEqual(propertyExpression, filterValue);
                    break;
                case (int)BdtFilterOperator.GreaterThan:
                    if (propertyExpression.Type == typeof(decimal?))
                    {
                        var hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                        var valueExpression = Expression.Property(propertyExpression, "Value");

                        filterExpression = Expression.AndAlso(hasValueExpression, Expression.GreaterThan(valueExpression, filterValue));
                    }
                    else
                    {
                        filterExpression = Expression.GreaterThan(propertyExpression, filterValue);
                    }
                    break;
                case (int)BdtFilterOperator.GreaterThanOrEquals:
                    if (propertyExpression.Type == typeof(decimal?))
                    {
                        var hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                        var valueExpression = Expression.Property(propertyExpression, "Value");

                        filterExpression = Expression.AndAlso(hasValueExpression, Expression.GreaterThanOrEqual(valueExpression, filterValue));
                    }
                    else
                    {
                        filterExpression = Expression.GreaterThanOrEqual(propertyExpression, filterValue);
                    }
                    break;
                case (int)BdtFilterOperator.LessThan:
                    if (propertyExpression.Type == typeof(decimal?))
                    {
                        var hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                        var valueExpression = Expression.Property(propertyExpression, "Value");

                        filterExpression = Expression.AndAlso(hasValueExpression, Expression.LessThan(valueExpression, filterValue));
                    }
                    else
                    {
                        filterExpression = Expression.LessThan(propertyExpression, filterValue);
                    }
                    break;
                case (int)BdtFilterOperator.LessThanOrEquals:
                    if (propertyExpression.Type == typeof(decimal?))
                    {
                        var hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                        var valueExpression = Expression.Property(propertyExpression, "Value");

                        filterExpression = Expression.AndAlso(hasValueExpression, Expression.LessThanOrEqual(valueExpression, filterValue));
                    }
                    else
                    {
                        filterExpression = Expression.LessThanOrEqual(propertyExpression, filterValue);
                    }
                    break;
                default:
                    return null;
            }

            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(filterExpression, parameterExpression);

            conditions[idx] = lambdaExpression;
            idx++;
        }

        return conditions;
    }
}
