using System.Linq.Expressions;

namespace Sarashop.service.IServices
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[] inclode = null, bool istrach = true);
        Task<T> GetOne(Expression<Func<T, bool>> expression, Expression<Func<T, object>>[] inclode = null, bool istrach = true);
        Task<T> AddAsync(T brand, CancellationToken cancellationToken = default);
        Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default);
    }
}
