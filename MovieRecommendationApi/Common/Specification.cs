using System.Linq.Expressions;

namespace MovieRecommendationApi.Common
{
    public class Specification<T> : ISpecification<T>
    {
        public List<Expression<Func<T, object>>> Includes { get; private set; } = [];

        public List<Expression<Func<T, bool>>> Criteria { get; private set; } = [];

        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; private set; }
        public int? PageSize { get; private set; }
        public int? PageNumber { get; private set; }

        public void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        public void AddCriteria(Expression<Func<T, bool>> expression)
        {
            Criteria.Add(expression);
        }

        public void AddOrderBy(string orderBy)
        {
            OrderBy = GetOrderByExpression(orderBy);
        }
        public void AddPaging(int pageNumber, int pageSize)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        private static Func<IQueryable<T>, IOrderedQueryable<T>> GetOrderByExpression(string orderBy)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            IOrderedQueryable<T>? ApplyOrdering(IQueryable<T> query, string propertyName, string direction, bool isThenBy)
            {
                var propertyExpression = Expression.Property(parameter, propertyName);
                var lambda = Expression.Lambda(propertyExpression, parameter);

                // Get the correct LINQ method
                var methodName = direction == "asc"
                    ? isThenBy ? "ThenBy" : "OrderBy"
                    : isThenBy ? "ThenByDescending" : "OrderByDescending";
                var method = typeof(Queryable).GetMethods()
                    .Single(m => m.Name == methodName
                                 && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), propertyExpression.Type);

                return (IOrderedQueryable<T>)method.Invoke(null, new object[] { query, lambda })!;
            }

            // Split order by clauses
            var orderByParts = orderBy.Split(',')
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrEmpty(part))
                .ToList();

            if (!orderByParts.Any())
            {
                throw new ArgumentException("Order by clause cannot be empty.");
            }

            return query =>
            {
                IOrderedQueryable<T>? orderedQuery = null;

                foreach (var (index, part) in orderByParts.Select((p, i) => (i, p)))
                {
                    var (propertyName, direction) = ParseOrderByPart(part);
                    orderedQuery = ApplyOrdering(orderedQuery ?? query, propertyName, direction, index > 0);
                }

                return orderedQuery!;
            };
        }


        private static (string propertyName, string direction) ParseOrderByPart(string part)
        {
            var parts = part.Split(' ');
            if (parts.Length == 2 && (parts[1].ToLower() == "asc" || parts[1].ToLower() == "desc"))
            {
                return (parts[0], parts[1].ToLower());
            }

            throw new ArgumentException("Invalid order by clause.");
        }

    }
}
