using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MovieRecommendationApi.Common
{
    public abstract class Repository<TDbContext, T, TKey> : IRepository<T, TKey>
        where T : Entity<TKey>
        where TDbContext : DbContext

    {
        protected readonly TDbContext _dbContext;

        protected Repository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var criteria in specification.Criteria)
            {
                query = query.Where(criteria);
            }

            foreach (var include in specification.Includes)
            {
                query = query.Include(include);
            }

            if (specification.OrderBy != null)
            {
                query = specification.OrderBy(query);
            }

            if (specification.PageNumber.HasValue && specification.PageSize.HasValue)
            {
                query = query.Skip(specification.PageSize.Value * (specification.PageNumber.Value - 1));
                query = query.Take(specification.PageSize.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var criteria in specification.Criteria)
            {
                query = query.Where(criteria);
            }

            return await query.CountAsync(cancellationToken);
        }

        public Task<T?> GetByIdAsync(TKey id, List<Expression<Func<T, object>>>? Includes, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (Includes != null)
            {
                foreach (var include in Includes)
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);
        }
    }

}
