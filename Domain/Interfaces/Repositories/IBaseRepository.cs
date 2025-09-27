using System.Data.Common;
using System.Linq.Expressions;
using Domain.Interfaces.Repositories;

namespace Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T, DbT> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<T>> FindAsync(List<Expression<Func<T, bool>>> predicates,
            CancellationToken cancellationToken);
        Task<IEnumerable<T>> FindWithIncludesAsync(List<Expression<Func<T, bool>>> predicates,
            CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindWithIncludesAndPaginationAsync(
            List<Expression<Func<T, bool>>> predicates,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken,
            params Expression<Func<T, object>>[] includes);

        Task<int>  GetTotalCountAsync(
            List<Expression<Func<T, bool>>> predicates,
            CancellationToken cancellationToken,
            params Expression<Func<T, object>>[] includes);
        
        Task<bool> AddAsync(T entity, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken);
        Task<bool> DeleteBatchAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken);

        Task<IEnumerable<object>> FindAllUniqueDataInPropertiesAsync(
            List<Expression<Func<T, bool>>> predicates,
            Expression<Func<T, object>> propertySelector,
            CancellationToken cancellationToken,
            params Expression<Func<T, object>>[] includes);
    }
}
