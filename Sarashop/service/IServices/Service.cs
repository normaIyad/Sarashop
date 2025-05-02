using Microsoft.EntityFrameworkCore;
using Sarashop.DataBase;
using System.Linq.Expressions;

namespace Sarashop.service.IServices
{
    public class Service<T> : IService<T> where T : class
    {
        public readonly DatabaseConfigration _databaseConfigration;
        public readonly DbSet<T> _dbSet;
        public Service(DatabaseConfigration databaseConfigration)
        {
            _databaseConfigration = databaseConfigration;
            _dbSet = _databaseConfigration.Set<T>();
        }
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _databaseConfigration.AddAsync(entity, cancellationToken);
            await _databaseConfigration.SaveChangesAsync();
            return entity;
        }

        public async Task<T> GetOne(Expression<Func<T, bool>> expression, Expression<Func<T, object>>[] inclode = null, bool istrach = true)
        {
            var all = await GetAsync(expression, inclode, istrach);

            return all.FirstOrDefault();
        }
        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[] inclode = null, bool istrach = true)
        {
            IQueryable<T> entity = _dbSet;
            if (expression != null)
            {
                entity = entity.Where(expression);
            }
            if (inclode != null)
            {
                foreach (var inc in inclode)
                {
                    entity = entity.Include(inc);
                }
            }
            if (!istrach)
            {
                entity = entity.AsNoTracking();
            }
            return await entity.ToListAsync();
        }

        public async Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            T? entity = _dbSet.Find(id);
            if (entity == null)
            {
                return false;
            }
            _dbSet.Remove(entity);
            await _databaseConfigration.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
