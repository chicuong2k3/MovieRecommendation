using System.Linq.Expressions;

namespace MovieRecommendationApi.Common
{
    public interface IRepository<T, TKey> where T : Entity<TKey>
    {
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(TKey id, List<Expression<Func<T, object>>>? Includes = null, CancellationToken cancellationToken = default);
        void Add(T entity);
        void Remove(T entity);
        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
