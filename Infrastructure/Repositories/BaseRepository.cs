using Domain.Interfaces.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T, DbT>(DbT context)
        : IBaseRepository<T, DbT> where T : class where DbT : DbContext
    {
        private readonly DbT _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                await _dbSet.AddAsync(entity, cancellationToken);
                // Save changes asynchronously and return true if successful
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding entity: {ex.Message}");
                // Log the exception or handle it as needed
                return false;
            }
        }

        public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                _dbSet.Remove(entity);
                // Save changes asynchronously and return true if successful
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting entity: {ex.Message}");
                // Log the exception or handle it as needed
                return false;
            }
        }

        public async Task<bool> DeleteBatchAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            try
            {
                _dbSet.RemoveRange(entities);
                // Save changes asynchronously and return true if successful
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting batch of entities: {ex.Message}");
                // Log the exception or handle it as needed
                return false;
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving entities: {ex.Message}");
                // Log the exception or handle it as needed
                return [];
            }
        }

        public async Task<IEnumerable<T>> FindWithIncludesAndPaginationAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet.Where(predicate);
                
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving entities with includes and pagination: {ex.Message}");
                // Log the exception or handle it as needed
                return [];
            }
        }

        public Task<IEnumerable<T>> FindWithIncludesAndPaginationAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet.Where(predicate);

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving entities with includes: {ex.Message}");
                // Log the exception or handle it as needed
                return [];
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _dbSet.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all entities: {ex.Message}");
                // Log the exception or handle it as needed
                return [];
            }
        }

        public async Task<(int TotalCount, int TotalPages)> GetTotalCountAndPagesAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, int pageSize, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet.Where(predicate);

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                var totalCount = await query.CountAsync(cancellationToken);
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                
                return (totalCount, totalPages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating total count and pages: {ex.Message}");
                // Log the exception or handle it as needed
                return (0, 0);
            }
        }

        public Task<(int TotalCount, int TotalPages)> GetTotalCountAndPagesAsync(Expression<Func<T, bool>> predicate, int pageSize, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                _dbSet.Update(entity);
                // Save changes asynchronously and return true if successful
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating entity: {ex.Message}");
                // Log the exception or handle it as needed
                return false;
            }
        }
    }
}
