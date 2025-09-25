using Domain.Interfaces.Repositories;
using Domain.Models.Abstract;
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

        public async Task<IEnumerable<T>> FindAsync(List<Expression<Func<T, bool>>> predicates, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                foreach (var predicate in predicates)
                    query = query.Where(predicate);

                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving entities: {ex.Message}");
                // Log the exception or handle it as needed
                return [];
            }
        }

        public async Task<IEnumerable<T>> FindWithIncludesAndPaginationAsync(List<Expression<Func<T, bool>>> predicates,
                                                                             int pageNumber, int pageSize,
                                                                             CancellationToken cancellationToken,
                                                                             params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                    query = query.Include(include);

                foreach (var predicate in predicates)
                    query = query.Where(predicate);
                
                if (typeof(BaseModel).IsAssignableFrom(typeof(T)))
                {
                    var propertyInfo = typeof(T).GetProperty("CreatedAt");

                    if (propertyInfo != null)
                    {
                        query = query.OrderByDescending(e => EF.Property<DateTime>(e, "CreatedAt"));
                    }
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


        public async Task<IEnumerable<T>> FindWithIncludesAsync(List<Expression<Func<T, bool>>> predicates,
                                                                CancellationToken cancellationToken,
                                                                params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                    query = query.Include(include);

                foreach (var predicate in predicates)
                    query = query.Where(predicate);

                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving entities with includes: {ex.Message}");
                // Log the exception or handle it as needed
                return [];
            }
        }

        public async Task<IEnumerable<object>> FindAllUniqueDataInPropertiesAsync(List<Expression<Func<T, bool>>> predicates,
                                                                             Expression<Func<T, object>> propertySelector,
                                                                             CancellationToken cancellationToken,
                                                                             params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                    query = query.Include(include);

                foreach (var predicate in predicates)
                    query = query.Where(predicate);

                return await query.Select(propertySelector).Distinct().ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving unique property data: {ex.Message}");
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

        public async Task<int> GetTotalCountAsync(List<Expression<Func<T, bool>>> predicates,
                                                  CancellationToken cancellationToken, 
                                                  params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                    query = query.Include(include);

                foreach (var predicate in predicates)
                    query = query.Where(predicate);

                int totalCount = await query.CountAsync(cancellationToken);

                return totalCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating total count and pages: {ex.Message}");
                // Log the exception or handle it as needed
                return 0;
            }
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
