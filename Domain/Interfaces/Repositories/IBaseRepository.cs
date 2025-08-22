using System.Data.Common;
using System.Linq.Expressions;
using Domain.Interfaces.Repositories;

namespace Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T, DbT> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken);
        Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindWithIncludesAndPaginationAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken,
            int pageNumber,
            int pageSize,
            params Expression<Func<T, object>>[] includes);
        Task<bool> AddAsync(T entity, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken);
        Task<bool> DeleteBatchAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken);
    }
}
