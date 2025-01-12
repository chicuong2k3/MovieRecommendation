using System.Linq.Expressions;

namespace MovieRecommendationApi.Common
{
    public interface ISpecification<T>
    {
        List<Expression<Func<T, bool>>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; }
        int? PageSize { get; }
        int? PageNumber { get; }
    }
}
